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
        public DbSet<User> Users { get; set; }
        public DbSet<TattooRequest> TattooRequests { get; set; }
        public DbSet<FormField> FormFields { get; set; }
        public DbSet<TattooRequestAnswer> TattooRequestAnswers { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}