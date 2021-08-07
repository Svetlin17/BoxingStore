namespace BoxingStore.Infrastructure
{
    using AutoMapper;
    using BoxingStore.Data.Models;
    using BoxingStore.Services.Products;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Product, LatestProductServiceModel>();

            this.CreateMap<ProductDetailsServiceModel, ProductFormServiceModel>();
        }
    }
}