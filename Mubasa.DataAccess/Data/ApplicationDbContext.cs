using Microsoft.EntityFrameworkCore;
using Mubasa.Models;

namespace Mubasa.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}
