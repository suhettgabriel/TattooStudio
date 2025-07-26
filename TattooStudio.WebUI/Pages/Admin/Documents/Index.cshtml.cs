using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages.Admin.Documents
{
    public class IndexModel : PageModel
    {
        private readonly ISharedDocumentRepository _docRepo;
        private readonly IFileStorageService _fileStorage;

        public IndexModel(ISharedDocumentRepository docRepo, IFileStorageService fileStorage)
        {
            _docRepo = docRepo;
            _fileStorage = fileStorage;
        }

        public IList<SharedDocument> Documents { get; set; } = new List<SharedDocument>();

        [BindProperty, Required(ErrorMessage = "Por favor, selecione um arquivo.")]
        public IFormFile UploadedFile { get; set; }

        public async Task OnGetAsync()
        {
            Documents = await _docRepo.GetAllAsync();
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            if (!ModelState.IsValid)
            {
                Documents = await _docRepo.GetAllAsync();
                return Page();
            }

            var fileUrl = await _fileStorage.SaveFileAsync(UploadedFile, "documents");

            var newDoc = new SharedDocument
            {
                FileName = UploadedFile.FileName,
                FileUrl = fileUrl
            };

            await _docRepo.AddAsync(newDoc);
            TempData["SuccessMessage"] = "Documento enviado com sucesso!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var docToDelete = await _docRepo.GetByIdAsync(id);
            if (docToDelete != null)
            {
                _fileStorage.DeleteFile(docToDelete.FileUrl);
                await _docRepo.DeleteAsync(id);
                TempData["SuccessMessage"] = "Documento excluído com sucesso!";
            }
            return RedirectToPage();
        }
    }
}