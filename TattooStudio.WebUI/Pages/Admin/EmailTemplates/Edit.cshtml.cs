using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages.Admin.EmailTemplates
{
    public class EditModel : PageModel
    {
        private readonly IEmailTemplateRepository _templateRepo;

        public EditModel(IEmailTemplateRepository templateRepo)
        {
            _templateRepo = templateRepo;
        }

        [BindProperty]
        public EmailTemplate Template { get; set; }

        public async Task<IActionResult> OnGetAsync(EmailTemplateType type)
        {
            Template = await _templateRepo.GetByTypeAsync(type);
            if (Template == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _templateRepo.UpdateAsync(Template);
            TempData["SuccessMessage"] = "Template salvo com sucesso!";
            return RedirectToPage("./Index");
        }
    }
}