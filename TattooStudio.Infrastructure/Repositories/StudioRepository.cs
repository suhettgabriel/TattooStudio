using Microsoft.EntityFrameworkCore;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.Infrastructure.Repositories
{
    public class StudioRepository : IStudioRepository
    {
        private readonly AppDbContext _context;

        public StudioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Studio?> GetByIdAsync(int id)
        {
            return await _context.Studios.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IList<Studio>> GetAllAsync()
        {
            return await _context.Studios.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(Studio studio)
        {
            await _context.Studios.AddAsync(studio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Studio studio)
        {
            _context.Studios.Update(studio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var studio = await _context.Studios.FindAsync(id);
            if (studio != null)
            {
                _context.Studios.Remove(studio);
                await _context.SaveChangesAsync();
            }
        }
    }
}