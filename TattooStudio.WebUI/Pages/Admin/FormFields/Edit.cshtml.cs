using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

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

            await _repository.UpdateAsync(FormField);
            TempData["SuccessMessage"] = "Campo do formulário atualizado com sucesso!";
            return RedirectToPage("./Index");
        }

        private void PopulateSelectLists()
        {
            var fieldTypes = from FormFieldType e in Enum.GetValues(typeof(FormFieldType))
                             where e != FormFieldType.UploadArquivo
                             select new
                             {
                                 Id = e,
                                 Name = e.GetType().GetMember(e.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.GetName() ?? e.ToString()
                             };
            FieldTypeOptions = new SelectList(fieldTypes, "Id", "Name", FormField.FieldType);
        }
    }
}