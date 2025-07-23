using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.WebUI.ViewModels;

namespace TattooStudio.WebUI.Pages
{
    public class IndexModel : PageModel
    {
        public List<GalleryImageViewModel> GalleryImages { get; set; } = new();

        public void OnGet()
        {
            LoadGalleryImages();
        }

        private void LoadGalleryImages()
        {
            GalleryImages = new List<GalleryImageViewModel>
            {
                new GalleryImageViewModel("/images/gallery/tattoo-1.jpeg", "Tatuagem de flor no antebraço"),
                new GalleryImageViewModel("/images/gallery/tattoo-2.jpeg", "Tatuagem de tigre nas costas"),
                new GalleryImageViewModel("/images/gallery/tattoo-3.jpeg", "Tatuagem de leão no braço"),
                new GalleryImageViewModel("/images/gallery/tattoo-4.jpeg", "Tatuagem de pássaro delicado"),
                new GalleryImageViewModel("/images/gallery/tattoo-5.jpeg", "Tatuagem de dragão na perna"),
                new GalleryImageViewModel("/images/gallery/tattoo-6.jpeg", "Tatuagem de mandala no ombro"),
                new GalleryImageViewModel("/images/gallery/tattoo-7.jpeg", "Tatuagem de frase na costela"),
                new GalleryImageViewModel("/images/gallery/tattoo-8.jpeg", "Tatuagem de lobo na coxa")
            };
        }
    }
}