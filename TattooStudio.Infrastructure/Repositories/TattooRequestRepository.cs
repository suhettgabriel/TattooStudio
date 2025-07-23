using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.Infrastructure.Repositories
{
    public class TattooRequestRepository : ITattooRequestRepository
    {
        private readonly AppDbContext _context;

        public TattooRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateRequestAsync(TattooRequest request)
        {
            _context.Users.Add(request.User);
            _context.TattooRequests.Add(request);
            await _context.SaveChangesAsync();
        }
    }
}