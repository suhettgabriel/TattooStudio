using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages.Admin.Gallery
{
    public class IndexModel : PageModel
    {
        private readonly IGalleryImageRepository _galleryRepo;
        private readonly IFileStorageService _fileStorage;

        public IndexModel(IGalleryImageRepository galleryRepo, IFileStorageService fileStorage)
        {
            _galleryRepo = galleryRepo;
            _fileStorage = fileStorage;
        }

        public IList<GalleryImage> Images { get; set; } = new List<GalleryImage>();

        [BindProperty]
        public IFormFile UploadedImage { get; set; }

        [BindProperty]
        public string? Description { get; set; }

        public async Task OnGetAsync()
        {
            Images = await _galleryRepo.GetAllAsync();
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            if (UploadedImage == null || UploadedImage.Length == 0)
            {
                ModelState.AddModelError("UploadedImage", "Por favor, selecione um arquivo de imagem.");
                Images = await _galleryRepo.GetAllAsync();
                return Page();
            }

            var imageUrl = await _fileStorage.SaveFileAsync(UploadedImage, "gallery");

            if (string.IsNullOrEmpty(imageUrl))
            {
                TempData["ErrorMessage"] = "Falha ao salvar a imagem.";
                return RedirectToPage();
            }

            var newImage = new GalleryImage
            {
                ImageUrl = imageUrl,
                Description = Description
            };

            await _galleryRepo.AddAsync(newImage);
            TempData["SuccessMessage"] = "Imagem adicionada à galeria com sucesso!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var imageToDelete = await _galleryRepo.GetByIdAsync(id);
            if (imageToDelete != null)
            {
                _fileStorage.DeleteFile(imageToDelete.ImageUrl, "gallery");
                await _galleryRepo.DeleteAsync(id);
                TempData["SuccessMessage"] = "Imagem removida da galeria com sucesso!";
            }
            else
            {
                TempData["ErrorMessage"] = "Imagem não encontrada.";
            }

            return RedirectToPage();
        }
    }
}