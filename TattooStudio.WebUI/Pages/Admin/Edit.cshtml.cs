using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages.Admin.Studios
{
    public class EditModel : PageModel
    {
        private readonly IStudioRepository _repository;

        public EditModel(IStudioRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public Studio Studio { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var studio = await _repository.GetByIdAsync(id.Value);
            if (studio == null) return NotFound();

            Studio = studio;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _repository.UpdateAsync(Studio);

            TempData["SuccessMessage"] = "Estúdio atualizado com sucesso!";
            return RedirectToPage("./Index");
        }
    }
}