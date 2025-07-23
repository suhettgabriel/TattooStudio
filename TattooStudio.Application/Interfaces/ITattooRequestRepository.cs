using TattooStudio.Core.Entities;
using TattooStudio.Core.Enums;

namespace TattooStudio.Application.Interfaces
{
    public interface ITattooRequestRepository
    {
        Task CreateRequestAsync(TattooRequest request);
        Task<IList<TattooRequest>> GetAllRequestsAsync(string? searchTerm, int? studioId, DateTime? startDate, DateTime? endDate);
        Task<TattooRequest?> GetRequestByIdAsync(int requestId);
        Task UpdateStatusAsync(int requestId, RequestStatus newStatus);
        Task UpdateAsync(TattooRequest request);
        Task DeleteAsync(int id);
    }
}