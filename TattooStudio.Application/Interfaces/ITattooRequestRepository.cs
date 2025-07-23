using TattooStudio.Core.Entities;

namespace TattooStudio.Application.Interfaces
{
    public interface ITattooRequestRepository
    {
        Task CreateRequestAsync(TattooRequest request);
    }
}