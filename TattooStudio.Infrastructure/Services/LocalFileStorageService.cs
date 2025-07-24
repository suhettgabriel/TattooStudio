using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TattooStudio.Application.Interfaces;

namespace TattooStudio.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;

        public LocalFileStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subfolder)
        {
            if (file == null || file.Length == 0)
            {
                return string.Empty;
            }

            var uploadsRootFolder = Path.Combine(_env.WebRootPath, "uploads");
            var targetFolder = Path.Combine(uploadsRootFolder, subfolder);

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(targetFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var fileUrl = $"uploads/{subfolder}/{uniqueFileName}";
            return fileUrl.Replace('\\', '/');
        }
    }
}