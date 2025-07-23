using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.WebUI.Pages.Admin.Requests
{
    public class IndexModel : PageModel
    {
        private readonly ITattooRequestRepository _requestRepo;
        private readonly IStudioRepository _studioRepo;
        private readonly AppDbContext _context;

        public IndexModel(ITattooRequestRepository requestRepo, IStudioRepository studioRepo, AppDbContext context)
        {
            _requestRepo = requestRepo;
            _studioRepo = studioRepo;
            _context = context;
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
        public List<string>? SelectedStatuses { get; set; }

        public SelectList StudioOptions { get; set; }
        public Dictionary<string, List<TattooRequest>> RequestsByStatus { get; set; } = new();
        public List<string> StatusColumns { get; set; } = new List<string>
        {
            "Nova Solicitação", "Em Análise", "Orçamento Enviado", "Aguardando Sinal",
            "Agendado", "Lista de Espera", "Recusado"
        };

        public async Task OnGetAsync()
        {
            if (SelectedStatuses == null || !SelectedStatuses.Any())
            {
                SelectedStatuses = new List<string>(StatusColumns);
            }
            await LoadRequestsAsync();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync([FromBody] UpdateStatusModel model)
        {
            if (model == null || model.RequestId <= 0 || string.IsNullOrEmpty(model.NewStatus))
            {
                return BadRequest();
            }

            await _requestRepo.UpdateRequestStatusAsync(model.RequestId, model.NewStatus);
            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteRequestAsync(int id)
        {
            var request = await _context.TattooRequests.Include(r => r.Answers).FirstOrDefaultAsync(r => r.Id == id);
            if (request != null)
            {
                var user = await _context.Users.FindAsync(request.UserId);
                _context.TattooRequestAnswers.RemoveRange(request.Answers);
                _context.TattooRequests.Remove(request);
                if (user != null)
                {
                    _context.Users.Remove(user);
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Solicitação excluída com sucesso.";
            }
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
        public string NewStatus { get; set; }
    }
}