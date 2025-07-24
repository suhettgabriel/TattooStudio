using Microsoft.EntityFrameworkCore;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.Infrastructure.Repositories
{
    public class GalleryImageRepository : IGalleryImageRepository
    {
        private readonly AppDbContext _context;

        public GalleryImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<GalleryImage>> GetAllAsync()
        {
            return await _context.GalleryImages.OrderBy(i => i.DisplayOrder).ToListAsync();
        }

        public async Task<GalleryImage?> GetByIdAsync(int id)
        {
            return await _context.GalleryImages.FindAsync(id);
        }

        public async Task AddAsync(GalleryImage image)
        {
            var maxOrder = await _context.GalleryImages.MaxAsync(i => (int?)i.DisplayOrder) ?? 0;
            image.DisplayOrder = maxOrder + 1;
            await _context.GalleryImages.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var image = await _context.GalleryImages.FindAsync(id);
            if (image != null)
            {
                _context.GalleryImages.Remove(image);
                await _context.SaveChangesAsync();
            }
        }
    }
}