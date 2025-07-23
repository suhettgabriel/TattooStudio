using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages.Admin.FormFields
{
    public class IndexModel : PageModel
    {
        private readonly IFormFieldRepository _repository;

        public IndexModel(IFormFieldRepository repository)
        {
            _repository = repository;
        }

        public IList<FormField> FormFields { get; set; } = new List<FormField>();

        public async Task OnGetAsync()
        {
            await LoadFormFieldsAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            TempData["SuccessMessage"] = "Campo do formulário excluído com sucesso!";
            return RedirectToPage("./Index");
        }

        private async Task LoadFormFieldsAsync()
        {
            FormFields = await _repository.GetAllAsync();
        }

        public async Task<IActionResult> OnPostReorderAsync([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("Nenhum ID fornecido.");
            }

            await _repository.UpdateOrderAsync(ids);
            return new OkResult();
        }
    }
}