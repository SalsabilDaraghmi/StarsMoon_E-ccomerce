using E_ccomerce_Task2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_ccomerce_Task2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<ProductCategory> productCategories { get; set; }
        public DbSet<Orders>  orders { get; set; }
        public DbSet<WishList> wishList { get; set; }
        public DbSet<ResetPasswordLog> resetPasswordLog { get; set; }
    }
}