using TattooStudio.Core.Entities;

namespace TattooStudio.Application.Interfaces
{
    public interface IQuoteRepository
    {
        Task<Quote?> GetByIdAsync(int id);
        Task UpdateAsync(Quote quote);
    }
}