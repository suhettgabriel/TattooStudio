using TattooStudio.Core.Entities;

namespace TattooStudio.Application.Interfaces
{
    public interface IStudioRepository
    {
        Task<Studio?> GetByIdAsync(int id);
        Task<IList<Studio>> GetAllAsync();
        Task AddAsync(Studio studio);
        Task UpdateAsync(Studio studio);
        Task DeleteAsync(int id);
    }
}