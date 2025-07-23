using Microsoft.EntityFrameworkCore;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.Infrastructure.Repositories
{
    public class FormFieldRepository : IFormFieldRepository
    {
        private readonly AppDbContext _context;

        public FormFieldRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FormField?> GetByIdAsync(int id)
        {
            return await _context.FormFields.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IList<FormField>> GetAllAsync()
        {
            return await _context.FormFields.AsNoTracking().OrderBy(f => f.Order).ToListAsync();
        }

        public async Task AddAsync(FormField formField)
        {
            await _context.FormFields.AddAsync(formField);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FormField formField)
        {
            _context.FormFields.Update(formField);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var formField = await _context.FormFields.FindAsync(id);
            if (formField != null)
            {
                _context.FormFields.Remove(formField);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetNextOrderValueAsync()
        {
            var maxOrder = await _context.FormFields.MaxAsync(f => (int?)f.Order) ?? 0;
            return maxOrder + 10;
        }

        public async Task UpdateOrderAsync(List<int> orderedIds)
        {
            var fields = await _context.FormFields.Where(f => orderedIds.Contains(f.Id)).ToListAsync();
            for (int i = 0; i < orderedIds.Count; i++)
            {
                var field = fields.FirstOrDefault(f => f.Id == orderedIds[i]);
                if (field != null)
                {
                    field.Order = (i + 1) * 10;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}