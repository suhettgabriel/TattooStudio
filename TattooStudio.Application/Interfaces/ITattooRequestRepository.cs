using TattooStudio.Core.Entities;

namespace TattooStudio.Application.Interfaces
{
    public interface ITattooRequestRepository
    {
        Task CreateRequestAsync(TattooRequest request);
        Task<IList<TattooRequest>> GetAllRequestsAsync(string? searchTerm, int? studioId, DateTime? startDate, DateTime? endDate);
        Task UpdateRequestStatusAsync(int requestId, string newStatus);
        Task<TattooRequest?> GetRequestByIdAsync(int requestId);
    }
}