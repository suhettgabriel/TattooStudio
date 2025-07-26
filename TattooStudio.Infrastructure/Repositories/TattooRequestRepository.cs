using Microsoft.EntityFrameworkCore;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Core.Enums;
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
            if (request.User != null)
            {
                _context.Users.Add(request.User);
            }
            _context.TattooRequests.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<TattooRequest>> GetAllRequestsAsync(string? searchTerm, int? studioId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.TattooRequests
                                .Include(r => r.User)
                                .Include(r => r.Studio)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var term = searchTerm.ToLower();
                query = query.Where(r => r.User != null && (r.User.FullName.ToLower().Contains(term) || r.User.Email.ToLower().Contains(term)));
            }

            if (studioId.HasValue && studioId.Value > 0)
            {
                query = query.Where(r => r.StudioId == studioId.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(r => r.SubmissionDate.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(r => r.SubmissionDate.Date <= endDate.Value.Date);
            }

            return await query.OrderByDescending(r => r.SubmissionDate).ToListAsync();
        }

        public async Task<TattooRequest?> GetRequestByIdAsync(int requestId)
        {
            return await _context.TattooRequests
                                 .Include(r => r.User)
                                 .Include(r => r.Studio)
                                 .Include(r => r.Quotes)
                                 .Include(r => r.ChatMessages)
                                 .Include(r => r.Appointment)
                                 .Include(r => r.Answers)
                                     .ThenInclude(a => a.FormField)
                                 .FirstOrDefaultAsync(r => r.Id == requestId);
        }

        public async Task UpdateStatusAsync(int requestId, RequestStatus newStatus)
        {
            var request = await _context.TattooRequests.FindAsync(requestId);
            if (request != null)
            {
                request.Status = newStatus;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(TattooRequest request)
        {
            _context.TattooRequests.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var request = await _context.TattooRequests.FindAsync(id);
            if (request != null)
            {
                _context.TattooRequests.Remove(request);
                await _context.SaveChangesAsync();
            }
        }
    }
}