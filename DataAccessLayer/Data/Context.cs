using EntityLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ecommerce.DataAccess.Data
{
    public class Context: IdentityDbContext<ApplicationUser>
    {
        public Context(DbContextOptions<Context> options): base(options)
        {
        }

        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(d => d.Discount)
                .WithOne(p => p.Product)
                .HasForeignKey<Discount>(d => d.ProductId);
            //    .HasPrincipalKey<Product>(p => p.DiscountId);

            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasNoKey();

            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(e => new { e.RoleId, e.UserId });

            modelBuilder.Entity<IdentityUserToken<string>>()
                .HasNoKey();
        }
	}
}
