using Microsoft.EntityFrameworkCore;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.Infrastructure.Repositories
{
    public class PricingRuleRepository : IPricingRuleRepository
    {
        private readonly AppDbContext _context;

        public PricingRuleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<PricingRule>> GetAllAsync()
        {
            return await _context.PricingRules.OrderBy(r => r.BodyPart).ThenBy(r => r.MinSize).ToListAsync();
        }

        public async Task<PricingRule?> GetByIdAsync(int id)
        {
            return await _context.PricingRules.FindAsync(id);
        }

        public async Task AddAsync(PricingRule rule)
        {
            await _context.PricingRules.AddAsync(rule);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PricingRule rule)
        {
            _context.PricingRules.Update(rule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rule = await _context.PricingRules.FindAsync(id);
            if (rule != null)
            {
                _context.PricingRules.Remove(rule);
                await _context.SaveChangesAsync();
            }
        }
    }
}