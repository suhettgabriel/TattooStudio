using TattooStudio.Core.Entities;

namespace TattooStudio.Application.Interfaces
{
    public interface IPricingRuleRepository
    {
        Task<IList<PricingRule>> GetAllAsync();
        Task<PricingRule?> GetByIdAsync(int id);
        Task AddAsync(PricingRule rule);
        Task UpdateAsync(PricingRule rule);
        Task DeleteAsync(int id);
    }
}