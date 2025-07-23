using TattooStudio.Core.Entities;

namespace TattooStudio.Application.Interfaces
{
    public interface IClientRepository
    {
        Task<IList<User>> GetAllAsync(string? searchTerm);
        Task<User?> GetByIdWithRelationsAsync(int id);
    }
}