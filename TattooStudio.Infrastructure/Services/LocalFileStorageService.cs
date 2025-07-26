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
                return string.Empty;

            var targetFolder = Path.Combine(_env.WebRootPath, "uploads", subfolder);
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(targetFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"uploads/{subfolder}/{uniqueFileName}".Replace('\\', '/');
        }

        public void DeleteFile(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl)) return;

            var filePath = Path.Combine(_env.WebRootPath, fileUrl.Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}