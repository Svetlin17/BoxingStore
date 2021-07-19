namespace BoxingStore.Data
{
    using BoxingStore.Data.Models;
    using BoxingStore.Data.Models.Enums;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class BoxingStoreDbContext : IdentityDbContext
    {
        public BoxingStoreDbContext(DbContextOptions<BoxingStoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; init; }

        public DbSet<Category> Categories { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Product>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder
            //    .Entity<Product>()
            //    .Property(e => e.Size)
            //    .HasConversion(
            //        v => v.ToString(),
            //        v => (ProductSize)Enum.Parse(typeof(ProductSize), v));


            base.OnModelCreating(builder);
        }
    }
}
