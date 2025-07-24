using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGalleryImageRepository _galleryRepo;

        public IndexModel(IGalleryImageRepository galleryRepo)
        {
            _galleryRepo = galleryRepo;
        }

        public IList<GalleryImage> GalleryImages { get; set; } = new List<GalleryImage>();

        public async Task OnGetAsync()
        {
            GalleryImages = await _galleryRepo.GetAllAsync();
        }
    }
}