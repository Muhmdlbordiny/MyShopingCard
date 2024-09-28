using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Myshop.Entities;
using Myshop.Entities.Models;

namespace Myshop.DataAccess.Data
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options) 
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>(p=>

               {
                 
                 p.Property(p => p.Name).HasColumnName("ProductName")
                            .HasColumnType("varchar").HasMaxLength(50);
                p.Property(p => p.price).HasColumnType("money").IsRequired();
                   p.Property(p => p.Description).HasColumnType("varchar").HasMaxLength(50);
                   p.Property(p => p.Image).HasColumnType("varchar").HasMaxLength(100);

                });
            //modelBuilder.Entity<ShopingCard>(s =>
            //{
            //    s.Property(s => s.Id).UseIdentityColumn(10, 10);
            //});
            //modelBuilder.Entity<ApplicationUsers>(c => { c.Property(c => c.Id).UseIdentityColumn(1, 1); });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet< Category>Categories { get;  set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ApplicationUsers> ApplicationUsers { get; set; }
        public DbSet<ShopingCard> ShopingCards { get; set; }
    }
}
