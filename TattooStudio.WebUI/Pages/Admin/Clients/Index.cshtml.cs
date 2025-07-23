using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages.Admin.Clients
{
    public class IndexModel : PageModel
    {
        private readonly IClientRepository _clientRepo;

        public IndexModel(IClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public IList<User> Clients { get; set; } = new List<User>();

        public async Task OnGetAsync()
        {
            Clients = await _clientRepo.GetAllAsync(SearchTerm);
        }
    }
}