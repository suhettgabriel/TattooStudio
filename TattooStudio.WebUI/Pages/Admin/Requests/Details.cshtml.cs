using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Core.Enums;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.WebUI.Pages.Admin.Requests
{
    public class DetailsModel : PageModel
    {
        private readonly ITattooRequestRepository _requestRepo;
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly AppDbContext _context;

        public DetailsModel(ITattooRequestRepository requestRepo, IAppointmentRepository appointmentRepo, AppDbContext context)
        {
            _requestRepo = requestRepo;
            _appointmentRepo = appointmentRepo;
            _context = context;
        }

        [BindProperty]
        public Quote NewQuote { get; set; } = new();

        public TattooRequest TattooRequest { get; set; }
        public bool HasExistingAppointment { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            return await LoadPageDataAsync(id.Value);
        }

        public async Task<IActionResult> OnPostScheduleAsync(int tattooRequestId, DateTime scheduleStart, int durationHours)
        {
            var tattooRequest = await _requestRepo.GetRequestByIdAsync(tattooRequestId);
            if (tattooRequest == null)
            {
                TempData["ErrorMessage"] = "Solicitação não encontrada.";
                return RedirectToPage("./Index");
            }

            if (await _appointmentRepo.ExistsByTattooRequestIdAsync(tattooRequestId))
            {
                TempData["ErrorMessage"] = "Esta solicitação já possui um agendamento.";
                return RedirectToPage(new { id = tattooRequestId });
            }

            await CreateNewAppointment(tattooRequest, scheduleStart, durationHours);
            TempData["SuccessMessage"] = "Sessão agendada com sucesso!";
            return RedirectToPage(new { id = tattooRequestId });
        }

        public async Task<IActionResult> OnPostMarkAsAnalyzingAsync(int requestId)
        {
            await _requestRepo.UpdateStatusAsync(requestId, RequestStatus.EmAnalise);
            TempData["SuccessMessage"] = "Status alterado para 'Em Análise'.";
            return RedirectToPage(new { id = requestId });
        }

        public async Task<IActionResult> OnPostCreateQuoteAsync(int requestId)
        {
            await TryUpdateModelAsync(NewQuote, "NewQuote", q => q.Amount, q => q.DepositAmount, q => q.Description, q => q.ExpiryDate);
            if (!ModelState.IsValid)
            {
                return await LoadPageDataAsync(requestId);
            }

            NewQuote.TattooRequestId = requestId;
            _context.Quotes.Add(NewQuote);
            await _context.SaveChangesAsync();

            await _requestRepo.UpdateStatusAsync(requestId, RequestStatus.OrcamentoEnviado);

            TempData["SuccessMessage"] = "Orçamento criado e enviado com sucesso!";
            return RedirectToPage(new { id = requestId });
        }

        public async Task<IActionResult> OnPostSendMessageAsync(int requestId, [FromBody] MessageInputModel input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.Message))
            {
                return BadRequest(new { message = "A mensagem não pode estar vazia." });
            }

            var newChatMessage = new ChatMessage
            {
                TattooRequestId = requestId,
                Message = input.Message,
                Sender = "Admin"
            };

            _context.ChatMessages.Add(newChatMessage);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                message = newChatMessage.Message,
                sentAt = newChatMessage.SentAt.ToString("dd/MM/yy HH:mm"),
                sender = newChatMessage.Sender
            });
        }

        private async Task<IActionResult> LoadPageDataAsync(int requestId)
        {
            var request = await _requestRepo.GetRequestByIdAsync(requestId);
            if (request == null)
            {
                return NotFound();
            }

            TattooRequest = request;
            HasExistingAppointment = request.Appointment != null;
            NewQuote.ExpiryDate = DateTime.Today.AddDays(7);
            return Page();
        }

        private async Task CreateNewAppointment(TattooRequest request, DateTime start, int duration)
        {
            var newAppointment = new Appointment
            {
                TattooRequestId = request.Id,
                Title = $"Tattoo: {request.User?.FullName ?? "Cliente"}",
                Start = start,
                End = start.AddHours(duration),
            };

            await _appointmentRepo.AddAsync(newAppointment);
            await _requestRepo.UpdateStatusAsync(request.Id, RequestStatus.Agendado);
        }
    }

    public class MessageInputModel
    {
        public string Message { get; set; }
    }
}