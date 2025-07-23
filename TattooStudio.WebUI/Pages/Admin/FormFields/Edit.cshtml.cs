using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.WebUI.Helpers;

namespace TattooStudio.WebUI.Pages.Admin.FormFields
{
    public class EditModel : PageModel
    {
        private readonly IFormFieldRepository _repository;

        public EditModel(IFormFieldRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public FormField FormField { get; set; } = new();

        public SelectList FieldTypeOptions { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var formField = await _repository.GetByIdAsync(id.Value);
            if (formField == null) return NotFound();

            FormField = formField;
            PopulateSelectLists();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateSelectLists();
                return Page();
            }

            await UpdateFormFieldAsync();
            TempData["SuccessMessage"] = "Campo do formulário atualizado com sucesso!";
            return RedirectToPage("./Index");
        }

        private void PopulateSelectLists()
        {
            FieldTypeOptions = EnumHelpers.ToSelectList(FormField.FieldType);
        }

        private async Task UpdateFormFieldAsync()
        {
            await _repository.UpdateAsync(FormField);
        }
    }
}