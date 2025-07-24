using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages
{
    public class FaqModel : PageModel
    {
        private readonly IFaqRepository _faqRepo;

        public FaqModel(IFaqRepository faqRepo)
        {
            _faqRepo = faqRepo;
        }

        public IList<FaqItem> FaqItems { get; set; } = new List<FaqItem>();

        public async Task OnGetAsync()
        {
            FaqItems = await _faqRepo.GetAllAsync();
        }
    }
}