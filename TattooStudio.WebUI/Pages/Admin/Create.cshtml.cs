using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages.Admin.Studios
{
    public class CreateModel : PageModel
    {
        private readonly IStudioRepository _repository;

        public CreateModel(IStudioRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public Studio Studio { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _repository.AddAsync(Studio);

            TempData["SuccessMessage"] = "Estúdio cadastrado com sucesso!";
            return RedirectToPage("./Index");
        }
    }
}