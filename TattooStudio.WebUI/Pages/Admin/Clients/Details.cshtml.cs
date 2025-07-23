using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages.Admin.Clients
{
    public class DetailsModel : PageModel
    {
        private readonly IClientRepository _clientRepo;

        public DetailsModel(IClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
        }

        public User Client { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientRepo.GetByIdWithRelationsAsync(id.Value);
            if (client == null)
            {
                return NotFound();
            }

            Client = client;
            return Page();
        }
    }
}