using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGalleryImageRepository _galleryRepo;
        private readonly ISystemSettingRepository _settingsRepo;
        private readonly IFaqRepository _faqRepo;

        public IndexModel(IGalleryImageRepository galleryRepo, ISystemSettingRepository settingsRepo, IFaqRepository faqRepo)
        {
            _galleryRepo = galleryRepo;
            _settingsRepo = settingsRepo;
            _faqRepo = faqRepo;
        }

        public IList<GalleryImage> GalleryImages { get; set; } = new List<GalleryImage>();
        public IList<FaqItem> FaqItems { get; set; } = new List<FaqItem>();
        public bool IsAgendaOpen { get; set; }

        public async Task OnGetAsync()
        {
            var settings = await _settingsRepo.GetSettingsAsync();
            IsAgendaOpen = settings.IsAgendaOpen;

            GalleryImages = await _galleryRepo.GetAllAsync();

            var allFaqs = await _faqRepo.GetAllAsync();
            FaqItems = allFaqs.Take(4).ToList();
        }
    }
}