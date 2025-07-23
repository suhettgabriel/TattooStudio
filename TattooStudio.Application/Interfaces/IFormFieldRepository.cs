using TattooStudio.Core.Entities;

namespace TattooStudio.Application.Interfaces
{
    public interface IFormFieldRepository
    {
        Task<FormField?> GetByIdAsync(int id);
        Task<IList<FormField>> GetAllAsync();
        Task AddAsync(FormField formField);
        Task UpdateAsync(FormField formField);
        Task DeleteAsync(int id);
        Task<int> GetNextOrderValueAsync();
        Task UpdateOrderAsync(List<int> orderedIds);
    }
}