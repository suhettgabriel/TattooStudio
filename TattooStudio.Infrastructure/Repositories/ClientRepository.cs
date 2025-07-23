using Microsoft.EntityFrameworkCore;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<User>> GetAllAsync(string? searchTerm)
        {
            var query = _context.Users
                .Include(u => u.TattooRequests)
                .Where(u => u.TattooRequests.Any())
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var term = searchTerm.ToLower();
                query = query.Where(u => u.FullName.ToLower().Contains(term) || u.Email.ToLower().Contains(term));
            }

            return await query.OrderBy(u => u.FullName).ToListAsync();
        }

        public async Task<User?> GetByIdWithRelationsAsync(int id)
        {
            return await _context.Users
                .Include(u => u.TattooRequests)
                    .ThenInclude(tr => tr.Appointment)
                .Include(u => u.TattooRequests)
                    .ThenInclude(tr => tr.Studio)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}