﻿namespace BoxingStore.Data
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

        public DbSet<ProductSizeQuantity> ProductSizeQuantities { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Product>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<ProductSizeQuantity>()
                .HasOne(p => p.Product)
                .WithMany(p => p.ProductSizeQuantities)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
