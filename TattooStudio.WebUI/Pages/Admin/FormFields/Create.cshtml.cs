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
    public class CreateModel : PageModel
    {
        private readonly IFormFieldRepository _repository;

        public CreateModel(IFormFieldRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public FormField FormField { get; set; } = new();

        public SelectList FieldTypeOptions { get; set; }

        public async Task OnGetAsync()
        {
            await PopulateInitialDataAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateSelectLists();
                return Page();
            }

            await _repository.AddAsync(FormField);
            TempData["SuccessMessage"] = "Novo campo cadastrado com sucesso!";
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
            FieldTypeOptions = new SelectList(fieldTypes, "Id", "Name");
        }

        private async Task PopulateInitialDataAsync()
        {
            PopulateSelectLists();
            FormField.Order = await _repository.GetNextOrderValueAsync();
        }
    }
}