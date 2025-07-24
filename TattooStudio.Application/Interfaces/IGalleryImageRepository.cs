using TattooStudio.Core.Entities;

namespace TattooStudio.Application.Interfaces
{
    public interface IGalleryImageRepository
    {
        Task<IList<GalleryImage>> GetAllAsync();
        Task AddAsync(GalleryImage image);
        Task DeleteAsync(int id);
        Task<GalleryImage?> GetByIdAsync(int id);
    }
}