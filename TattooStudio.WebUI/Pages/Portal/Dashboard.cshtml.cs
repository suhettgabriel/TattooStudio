using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages.Portal
{
    public class DashboardModel : PageModel
    {
        private readonly ITattooRequestRepository _requestRepo;

        public DashboardModel(ITattooRequestRepository requestRepo)
        {
            _requestRepo = requestRepo;
        }

        public TattooRequest TattooRequest { get; set; }

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

            return Page();
        }
    }
}