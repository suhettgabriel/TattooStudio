using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Core.Enums;

namespace TattooStudio.WebUI.Pages.Portal
{
    public class DashboardModel : PageModel
    {
        private readonly ITattooRequestRepository _requestRepo;
        private readonly IQuoteRepository _quoteRepo;

        public DashboardModel(ITattooRequestRepository requestRepo, IQuoteRepository quoteRepo)
        {
            _requestRepo = requestRepo;
            _quoteRepo = quoteRepo;
        }

        public TattooRequest TattooRequest { get; set; }
        public Quote? PendingQuote { get; set; }

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

            PendingQuote = TattooRequest.Quotes
                .OrderByDescending(q => q.CreatedAt)
                .FirstOrDefault(q => q.Status == QuoteStatus.Pendente);

            return Page();
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
    }
}