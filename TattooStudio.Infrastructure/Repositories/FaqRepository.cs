using Microsoft.EntityFrameworkCore;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.Infrastructure.Repositories
{
    public class FaqRepository : IFaqRepository
    {
        private readonly AppDbContext _context;

        public FaqRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<FaqItem>> GetAllAsync()
        {
            return await _context.FaqItems.OrderBy(f => f.DisplayOrder).ToListAsync();
        }

        public async Task AddAsync(FaqItem faqItem)
        {
            var maxOrder = await _context.FaqItems.MaxAsync(f => (int?)f.DisplayOrder) ?? 0;
            faqItem.DisplayOrder = maxOrder + 1;
            await _context.FaqItems.AddAsync(faqItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var faqItem = await _context.FaqItems.FindAsync(id);
            if (faqItem != null)
            {
                _context.FaqItems.Remove(faqItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}