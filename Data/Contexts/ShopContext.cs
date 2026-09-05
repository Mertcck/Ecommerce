using Core.Concretes.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts
{
    public class ShopContext : IdentityDbContext<AppUser>
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<ProductFeature> ProductFeatures { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Birebir ilişkiler (Kimlik ve Profiller)
            builder.Entity<Admin>()
                   .HasOne(a => a.AppUser)
                   .WithOne(u => u.AdminProfile)
                   .HasForeignKey<Admin>(a => a.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade); // AppUser silinirse profil de silinecektir.

            builder.Entity<Customer>()
                   .HasOne(c => c.AppUser)
                   .WithOne(u => u.CustomerProfile)
                   .HasForeignKey<Customer>(c => c.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade); // AppUser silinirse profil de silinecektir.

            builder.Entity<Store>()
                   .HasOne(s => s.AppUser)
                   .WithOne(u => u.StoreProfile)
                   .HasForeignKey<Store>(s => s.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade); // AppUser silinirse profil de silinecektir.
            #endregion

            #region E-Ticaret ilişkileri
            // Kategori -> AltKategori
            builder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict); // AltKategori bulunduran Kategoriler silinemez.

            // Ürün -> Marka
            builder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ürün -> Kategori
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ürün -> Mağaza
            builder.Entity<Product>()
                .HasOne(p => p.Store)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ürün Görseli -> Ürün
            builder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ürün Özelliği -> Ürün
            builder.Entity<ProductFeature>()
                .HasOne(pf => pf.Product)
                .WithMany(p => p.Features)
                .HasForeignKey(pf => pf.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Sepet -> Müşteri
            builder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithMany(c => c.Carts)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Sepet Ürünü -> Sepet
            builder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // Sepet Ürünü -> Ürün
            builder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region AutoInclude
            builder.Entity<AppUser>()
                .Navigation(u => u.AdminProfile)
                .AutoInclude();

            builder.Entity<AppUser>()
                .Navigation(u => u.CustomerProfile)
                .AutoInclude();

            builder.Entity<AppUser>()
                .Navigation(u => u.StoreProfile)
                .AutoInclude();

            builder.Entity<Product>()
                .Navigation(p => p.Category)
                .AutoInclude();

            builder.Entity<Product>()
                .Navigation(p => p.Brand)
                .AutoInclude();

            builder.Entity<Product>()
                .Navigation(p => p.Images)
                .AutoInclude();

            builder.Entity<Product>()
                .Navigation(p => p.Features)
                .AutoInclude();

            builder.Entity<Product>()
                .Navigation(p => p.Store)
                .AutoInclude();

            builder.Entity<CartItem>()
                .Navigation(ci => ci.Product)
                .AutoInclude();

            builder.Entity<Cart>()
                .Navigation(c => c.Customer)
                .AutoInclude();

            builder.Entity<Cart>()
                .Navigation(c => c.Items)
                .AutoInclude();
            #endregion
        }
    }
}
