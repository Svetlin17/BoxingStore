namespace BoxingStore.Infrastructure
{
    using System.Linq;
    using BoxingStore.Data;
    using BoxingStore.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<BoxingStoreDbContext>();

            data.Database.Migrate();

            SeedCategories(data);

            return app;
        }

        private static void SeedCategories(BoxingStoreDbContext data)
        {
            if (data.Categories.Any())
            {
                return;
            }

            data.Categories.AddRange(new[]
            {
                new Category { Name = "Gloves" },
                new Category { Name = "Shorts" },
                new Category { Name = "Headgear" },
                new Category { Name = "Mouthguard" },
                new Category { Name = "Handwraps" },
            });

            data.SaveChanges();
        }
    }
}
