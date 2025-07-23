using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.WebUI.Pages.Admin.Requests
{
    public class DetailsModel : PageModel
    {
        private readonly ITattooRequestRepository _requestRepo;
        private readonly AppDbContext _context;

        public DetailsModel(ITattooRequestRepository requestRepo, AppDbContext context)
        {
            _requestRepo = requestRepo;
            _context = context;
        }

        [BindProperty]
        public Quote NewQuote { get; set; } = new();

        [BindProperty]
        public string NewMessage { get; set; }

        public TattooRequest TattooRequest { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _requestRepo.GetRequestByIdAsync(id.Value);
            if (request == null)
            {
                return NotFound();
            }

            TattooRequest = request;
            NewQuote.ExpiryDate = DateTime.Today.AddDays(7);
            return Page();
        }

        public async Task<IActionResult> OnPostCreateQuoteAsync(int requestId)
        {
            if (!ModelState.IsValid)
            {
                var request = await _requestRepo.GetRequestByIdAsync(requestId);
                if (request == null) return NotFound();
                TattooRequest = request;
                return Page();
            }

            await SaveQuoteAsync(requestId);

            TempData["SuccessMessage"] = "Orçamento criado com sucesso!";
            return RedirectToPage(new { id = requestId });
        }

        public async Task<IActionResult> OnPostSendMessageAsync(int requestId, [FromBody] MessageInputModel input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.Message))
            {
                return BadRequest(new { message = "A mensagem não pode estar vazia." });
            }

            var newChatMessage = await SaveMessageAsync(requestId, input.Message);

            return new JsonResult(new
            {
                message = newChatMessage.Message,
                sentAt = newChatMessage.SentAt.ToString("dd/MM/yy HH:mm"),
                sender = newChatMessage.Sender
            });
        }

        private async Task SaveQuoteAsync(int requestId)
        {
            NewQuote.TattooRequestId = requestId;
            _context.Quotes.Add(NewQuote);
            await _context.SaveChangesAsync();
        }

        private async Task<ChatMessage> SaveMessageAsync(int requestId, string messageContent)
        {
            var chatMessage = new ChatMessage
            {
                TattooRequestId = requestId,
                Message = messageContent,
                Sender = "Admin"
            };
            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();
            return chatMessage;
        }
    }

    public class MessageInputModel
    {
        public string Message { get; set; }
    }
}