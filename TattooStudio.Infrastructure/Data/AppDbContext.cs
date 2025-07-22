using Microsoft.EntityFrameworkCore;
using TattooStudio.Core.Entities;

namespace TattooStudio.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Studio> Studios { get; set; }
    }
}