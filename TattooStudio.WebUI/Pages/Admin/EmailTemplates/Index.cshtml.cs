using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages.Admin.EmailTemplates
{
    public class IndexModel : PageModel
    {
        private readonly IEmailTemplateRepository _templateRepo;

        public IndexModel(IEmailTemplateRepository templateRepo)
        {
            _templateRepo = templateRepo;
        }

        public IList<EmailTemplate> Templates { get; set; } = new List<EmailTemplate>();

        public async Task OnGetAsync()
        {
            Templates = await _templateRepo.GetAllAsync();
        }
    }
}