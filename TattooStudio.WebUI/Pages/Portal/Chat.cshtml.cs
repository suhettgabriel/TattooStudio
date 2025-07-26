using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.WebUI.Pages.Portal
{
    public class ChatModel : PageModel
    {
        private readonly ITattooRequestRepository _requestRepo;
        private readonly AppDbContext _context;

        public ChatModel(ITattooRequestRepository requestRepo, AppDbContext context)
        {
            _requestRepo = requestRepo;
            _context = context;
        }

        public TattooRequest TattooRequest { get; set; }

        [BindProperty]
        public string NewMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            return await LoadPageData();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var pageResult = await LoadPageData();
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(NewMessage))
            {
                return pageResult;
            }

            var newMessage = new ChatMessage
            {
                TattooRequestId = TattooRequest.Id,
                Message = NewMessage,
                Sender = "Client",
                IsRead = false
            };

            _context.ChatMessages.Add(newMessage);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Mensagem enviada com sucesso!";
            return RedirectToPage();
        }

        private async Task<IActionResult> LoadPageData()
        {
            var requestIdClaim = User.FindFirst("TattooRequestId");
            if (requestIdClaim == null || !int.TryParse(requestIdClaim.Value, out var requestId))
            {
                return Challenge();
            }

            TattooRequest = await _requestRepo.GetRequestByIdAsync(requestId);

            if (TattooRequest == null)
            {
                return NotFound("Sua solicitação não foi encontrada.");
            }

            return Page();
        }
    }
}