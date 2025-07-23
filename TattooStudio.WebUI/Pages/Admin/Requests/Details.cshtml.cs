using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages.Admin.Requests
{
    public class DetailsModel : PageModel
    {
        private readonly ITattooRequestRepository _requestRepo;

        public DetailsModel(ITattooRequestRepository requestRepo)
        {
            _requestRepo = requestRepo;
        }

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
            return Page();
        }
    }
}