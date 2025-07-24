using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Core.Enums;

namespace TattooStudio.WebUI.Pages.Admin.WaitingList
{
    public class IndexModel : PageModel
    {
        private readonly ITattooRequestRepository _requestRepo;

        public IndexModel(ITattooRequestRepository requestRepo)
        {
            _requestRepo = requestRepo;
        }

        public IList<TattooRequest> Entries { get; set; } = new List<TattooRequest>();

        public async Task OnGetAsync()
        {
            var allRequests = await _requestRepo.GetAllRequestsAsync(null, null, null, null);
            Entries = allRequests.Where(r => r.Status == RequestStatus.ListaDeEspera).ToList();
        }

        public async Task<IActionResult> OnPostConvertToRequestAsync(int id)
        {
            var request = await _requestRepo.GetRequestByIdAsync(id);
            if (request == null)
            {
                TempData["ErrorMessage"] = "Inscri��o n�o encontrada.";
                return RedirectToPage();
            }

            await _requestRepo.UpdateStatusAsync(id, RequestStatus.NovaSolicitacao);

            TempData["SuccessMessage"] = $"A inscri��o de '{request.User?.FullName}' foi movida para 'Nova Solicita��o' no Kanban.";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeclineAsync(int id)
        {
            var request = await _requestRepo.GetRequestByIdAsync(id);
            if (request == null)
            {
                TempData["ErrorMessage"] = "Inscri��o n�o encontrada.";
                return RedirectToPage();
            }

            await _requestRepo.UpdateStatusAsync(id, RequestStatus.Recusado);

            TempData["SuccessMessage"] = $"A inscri��o de '{request.User?.FullName}' foi movida para 'Recusado' no Kanban.";

            return RedirectToPage();
        }
    }
}