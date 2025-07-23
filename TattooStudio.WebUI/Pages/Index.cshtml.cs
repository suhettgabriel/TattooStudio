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
                new GalleryImageViewModel("/images/gallery/tattoo-1.jpeg", "Tatuagem de flor no antebra�o"),
                new GalleryImageViewModel("/images/gallery/tattoo-2.jpeg", "Tatuagem de tigre nas costas"),
                new GalleryImageViewModel("/images/gallery/tattoo-3.jpeg", "Tatuagem de le�o no bra�o"),
                new GalleryImageViewModel("/images/gallery/tattoo-4.jpeg", "Tatuagem de p�ssaro delicado"),
                new GalleryImageViewModel("/images/gallery/tattoo-5.jpeg", "Tatuagem de drag�o na perna"),
                new GalleryImageViewModel("/images/gallery/tattoo-6.jpeg", "Tatuagem de mandala no ombro"),
                new GalleryImageViewModel("/images/gallery/tattoo-7.jpeg", "Tatuagem de frase na costela"),
                new GalleryImageViewModel("/images/gallery/tattoo-8.jpeg", "Tatuagem de lobo na coxa")
            };
        }
    }
}