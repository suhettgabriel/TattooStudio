using Microsoft.EntityFrameworkCore;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.Infrastructure.Repositories
{
    public class SharedDocumentRepository : ISharedDocumentRepository
    {
        private readonly AppDbContext _context;

        public SharedDocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<SharedDocument>> GetAllAsync()
        {
            return await _context.SharedDocuments.OrderByDescending(d => d.UploadedAt).ToListAsync();
        }

        public async Task<SharedDocument?> GetByIdAsync(int id)
        {
            return await _context.SharedDocuments.FindAsync(id);
        }

        public async Task AddAsync(SharedDocument document)
        {
            await _context.SharedDocuments.AddAsync(document);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var document = await _context.SharedDocuments.FindAsync(id);
            if (document != null)
            {
                _context.SharedDocuments.Remove(document);
                await _context.SaveChangesAsync();
            }
        }
    }
}