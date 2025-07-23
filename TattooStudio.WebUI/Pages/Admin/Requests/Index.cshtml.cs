using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Core.Enums;
using TattooStudio.WebUI.Helpers;

namespace TattooStudio.WebUI.Pages.Admin.Requests
{
    public class IndexModel : PageModel
    {
        private readonly ITattooRequestRepository _requestRepo;
        private readonly IStudioRepository _studioRepo;

        public IndexModel(ITattooRequestRepository requestRepo, IStudioRepository studioRepo)
        {
            _requestRepo = requestRepo;
            _studioRepo = studioRepo;
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? StudioId { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Data Inicial")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Data Final")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<RequestStatus>? SelectedStatuses { get; set; }

        public SelectList StudioOptions { get; set; }
        public Dictionary<RequestStatus, List<TattooRequest>> RequestsByStatus { get; set; } = new();
        public List<RequestStatus> StatusColumns { get; set; } = Enum.GetValues(typeof(RequestStatus)).Cast<RequestStatus>().ToList();

        public async Task OnGetAsync()
        {
            if (SelectedStatuses == null || !SelectedStatuses.Any())
            {
                SelectedStatuses = new List<RequestStatus>(StatusColumns);
            }
            await LoadRequestsAsync();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync([FromBody] UpdateStatusModel model)
        {
            if (model == null || model.RequestId <= 0)
            {
                return BadRequest();
            }

            await _requestRepo.UpdateStatusAsync(model.RequestId, model.NewStatus);
            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteRequestAsync(int id)
        {
            await _requestRepo.DeleteAsync(id);
            TempData["SuccessMessage"] = "Solicitação excluída com sucesso.";
            return RedirectToPage();
        }

        private async Task LoadRequestsAsync()
        {
            var studios = await _studioRepo.GetAllAsync();
            StudioOptions = new SelectList(studios, nameof(Studio.Id), nameof(Studio.City));

            var allRequests = await _requestRepo.GetAllRequestsAsync(SearchTerm, StudioId, StartDate, EndDate);

            RequestsByStatus = SelectedStatuses.ToDictionary(
                status => status,
                status => allRequests.Where(r => r.Status == status).ToList()
            );
        }
    }

    public class UpdateStatusModel
    {
        public int RequestId { get; set; }
        public RequestStatus NewStatus { get; set; }
    }
}