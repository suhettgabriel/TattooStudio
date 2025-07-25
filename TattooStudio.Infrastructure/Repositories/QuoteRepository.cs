using Microsoft.EntityFrameworkCore;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.Infrastructure.Repositories
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly AppDbContext _context;

        public QuoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Quote?> GetByIdAsync(int id)
        {
            return await _context.Quotes.FindAsync(id);
        }

        public async Task UpdateAsync(Quote quote)
        {
            _context.Quotes.Update(quote);
            await _context.SaveChangesAsync();
        }
    }
}