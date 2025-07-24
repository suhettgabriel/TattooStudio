using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGalleryImageRepository _galleryRepo;
        private readonly ISystemSettingRepository _settingsRepo;

        public IndexModel(IGalleryImageRepository galleryRepo, ISystemSettingRepository settingsRepo)
        {
            _galleryRepo = galleryRepo;
            _settingsRepo = settingsRepo;
        }

        public IList<GalleryImage> GalleryImages { get; set; } = new List<GalleryImage>();
        public bool IsAgendaOpen { get; set; }

        public async Task OnGetAsync()
        {
            var settings = await _settingsRepo.GetSettingsAsync();
            IsAgendaOpen = settings.IsAgendaOpen;
            GalleryImages = await _galleryRepo.GetAllAsync();
        }
    }
}