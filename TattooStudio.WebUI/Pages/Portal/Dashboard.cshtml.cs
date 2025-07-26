using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Core.Enums;

namespace TattooStudio.WebUI.Pages.Portal
{
    public class DashboardModel : PageModel
    {
        private readonly ITattooRequestRepository _requestRepo;
        private readonly IQuoteRepository _quoteRepo;
        private readonly ISystemSettingRepository _settingsRepo;
        private readonly ISharedDocumentRepository _docRepo;

        public DashboardModel(
            ITattooRequestRepository requestRepo,
            IQuoteRepository quoteRepo,
            ISystemSettingRepository settingsRepo,
            ISharedDocumentRepository docRepo)
        {
            _requestRepo = requestRepo;
            _quoteRepo = quoteRepo;
            _settingsRepo = settingsRepo;
            _docRepo = docRepo;
        }

        public TattooRequest TattooRequest { get; set; }
        public Quote? PendingQuote { get; set; }
        public Appointment? ConfirmedAppointment { get; set; }
        public SystemSetting Settings { get; set; }
        public IList<SharedDocument> Documents { get; set; }

        [BindProperty]
        public bool IsConsentGiven { get; set; }

        public async Task<IActionResult> OnGetAsync()
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

            Settings = await _settingsRepo.GetSettingsAsync();
            Documents = await _docRepo.GetAllAsync();

            if (TattooRequest.Status == RequestStatus.OrcamentoEnviado)
            {
                PendingQuote = TattooRequest.Quotes.FirstOrDefault(q => q.Status == QuoteStatus.Pendente);
            }

            if (TattooRequest.Status == RequestStatus.Agendado)
            {
                ConfirmedAppointment = TattooRequest.Appointment;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostGiveConsentAsync(int requestId)
        {
            if (!IsConsentGiven)
            {
                ModelState.AddModelError("IsConsentGiven", "Você precisa aceitar os termos para continuar.");
                return await OnGetAsync();
            }

            var request = await _requestRepo.GetRequestByIdAsync(requestId);
            if (request == null)
            {
                return NotFound();
            }

            request.ConsentGiven = true;
            request.ConsentDate = DateTime.UtcNow;
            await _requestRepo.UpdateAsync(request);

            TempData["SuccessMessage"] = "Consentimento registrado com sucesso. Obrigado!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostApproveQuoteAsync(int requestId, int quoteId)
        {
            var quote = await _quoteRepo.GetByIdAsync(quoteId);
            if (quote == null || quote.TattooRequestId != requestId)
            {
                return Forbid();
            }

            quote.Status = QuoteStatus.Aprovado;
            await _quoteRepo.UpdateAsync(quote);

            await _requestRepo.UpdateStatusAsync(requestId, RequestStatus.AguardandoSinal);

            TempData["SuccessMessage"] = "Orçamento aprovado com sucesso! O próximo passo é o pagamento do sinal.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeclineQuoteAsync(int requestId, int quoteId)
        {
            var quote = await _quoteRepo.GetByIdAsync(quoteId);
            if (quote == null || quote.TattooRequestId != requestId)
            {
                return Forbid();
            }

            quote.Status = QuoteStatus.Recusado;
            await _quoteRepo.UpdateAsync(quote);

            await _requestRepo.UpdateStatusAsync(requestId, RequestStatus.Recusado);

            TempData["SuccessMessage"] = "Orçamento recusado. Sua solicitação foi movida para os arquivos.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetDownloadCalendarFile(int appointmentId)
        {
            var requestIdClaim = User.FindFirst("TattooRequestId");
            if (requestIdClaim == null || !int.TryParse(requestIdClaim.Value, out var requestId))
            {
                return Forbid();
            }

            var request = await _requestRepo.GetRequestByIdAsync(requestId);
            var appointment = request?.Appointment;

            if (appointment == null || appointment.Id != appointmentId)
            {
                return NotFound();
            }

            var calendarContent = new StringBuilder();
            calendarContent.AppendLine("BEGIN:VCALENDAR");
            calendarContent.AppendLine("VERSION:2.0");
            calendarContent.AppendLine("PRODID:-//TattooStudio//Appointment");
            calendarContent.AppendLine("BEGIN:VEVENT");
            calendarContent.AppendLine($"UID:{appointment.Id}@tattoostudio.com");
            calendarContent.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
            calendarContent.AppendLine($"DTSTART:{appointment.Start:yyyyMMddTHHmmss}");
            calendarContent.AppendLine($"DTEND:{appointment.End:yyyyMMddTHHmmss}");
            calendarContent.AppendLine($"SUMMARY:{appointment.Title}");
            calendarContent.AppendLine($"LOCATION:{request.Studio?.Address}, {request.Studio?.City}");
            calendarContent.AppendLine("END:VEVENT");
            calendarContent.AppendLine("END:VCALENDAR");

            var calendarBytes = Encoding.UTF8.GetBytes(calendarContent.ToString());
            return File(calendarBytes, "text/calendar", "agendamento-tatuagem.ics");
        }
    }
}