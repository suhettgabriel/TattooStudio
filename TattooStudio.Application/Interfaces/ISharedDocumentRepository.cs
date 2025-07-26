using TattooStudio.Core.Entities;

namespace TattooStudio.Application.Interfaces
{
    public interface ISharedDocumentRepository
    {
        Task<IList<SharedDocument>> GetAllAsync();
        Task<SharedDocument?> GetByIdAsync(int id);
        Task AddAsync(SharedDocument document);
        Task DeleteAsync(int id);
    }
}