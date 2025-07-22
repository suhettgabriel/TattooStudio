using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages.Admin.Studios
{
    public class IndexModel : PageModel
    {
        private readonly IStudioRepository _repository;

        public IndexModel(IStudioRepository repository)
        {
            _repository = repository;
        }

        public IList<Studio> Studios { get; set; } = new List<Studio>();

        public async Task OnGetAsync()
        {
            Studios = await _repository.GetAllAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            TempData["SuccessMessage"] = "Estúdio excluído com sucesso!";
            return RedirectToPage("./Index");
        }
    }
}