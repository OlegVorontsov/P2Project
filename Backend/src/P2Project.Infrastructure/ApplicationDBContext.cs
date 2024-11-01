using Microsoft.EntityFrameworkCore;
using P2Project.Domain.Models;

namespace P2Project.Infrastructure
{
    public class ApplicationDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("");
        }
        public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    }
}
