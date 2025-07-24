using TattooStudio.Core.Entities;

namespace TattooStudio.Application.Interfaces
{
    public interface IFaqRepository
    {
        Task<IList<FaqItem>> GetAllAsync();
        Task AddAsync(FaqItem faqItem);
        Task DeleteAsync(int id);
    }
}