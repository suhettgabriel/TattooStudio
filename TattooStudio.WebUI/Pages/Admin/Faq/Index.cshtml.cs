using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages.Admin.Faq
{
    public class IndexModel : PageModel
    {
        private readonly IFaqRepository _faqRepo;

        public IndexModel(IFaqRepository faqRepo)
        {
            _faqRepo = faqRepo;
        }

        public IList<FaqItem> FaqItems { get; set; } = new List<FaqItem>();

        [BindProperty]
        public FaqItem NewFaqItem { get; set; } = new();

        public async Task OnGetAsync()
        {
            FaqItems = await _faqRepo.GetAllAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                FaqItems = await _faqRepo.GetAllAsync();
                return Page();
            }

            await _faqRepo.AddAsync(NewFaqItem);
            TempData["SuccessMessage"] = "Novo item de FAQ adicionado com sucesso!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _faqRepo.DeleteAsync(id);
            TempData["SuccessMessage"] = "Item de FAQ excluído com sucesso!";
            return RedirectToPage();
        }
    }
}