using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages.Admin.Pricing
{
    public class IndexModel : PageModel
    {
        private readonly IPricingRuleRepository _pricingRepo;

        public IndexModel(IPricingRuleRepository pricingRepo)
        {
            _pricingRepo = pricingRepo;
        }

        public IList<PricingRule> Rules { get; set; } = new List<PricingRule>();

        [BindProperty]
        public PricingRule NewRule { get; set; } = new();

        public async Task OnGetAsync()
        {
            Rules = await _pricingRepo.GetAllAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Rules = await _pricingRepo.GetAllAsync();
                return Page();
            }

            await _pricingRepo.AddAsync(NewRule);
            TempData["SuccessMessage"] = "Nova regra de preço cadastrada com sucesso!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _pricingRepo.DeleteAsync(id);
            TempData["SuccessMessage"] = "Regra de preço excluída com sucesso!";
            return RedirectToPage();
        }
    }
}