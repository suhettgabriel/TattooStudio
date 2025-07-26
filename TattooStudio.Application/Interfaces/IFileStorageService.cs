﻿using Microsoft.AspNetCore.Http;

namespace TattooStudio.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string subfolder);
        void DeleteFile(string fileUrl);
    }
}