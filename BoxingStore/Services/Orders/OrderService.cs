namespace BoxingStore.Services.Orders
{
    using AutoMapper;
    using BoxingStore.Data;
    using BoxingStore.Data.Models;
    using BoxingStore.Models.Orders;
    using BoxingStore.Services.Products;
    using System.Linq;

    public class OrderService : IOrderService
    {
        private readonly BoxingStoreDbContext data;
        private readonly IConfigurationProvider mapper;

        public OrderService(BoxingStoreDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public Order Create()//OrderFormServiceModel order)
        {
            /*var productData = new Product
            {
                Brand = product.Brand,
                Name = product.Name,
                ConvertedName = convertedName,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId
            };*/

            return null;
        }

        public OrderQueryServiceModel All()
        {
            var ordersQuery = this.data.Orders.AsQueryable();

            var orders = ordersQuery
                .Select(o => new OrderServiceModel
                {
                    Id = o.Id,
                    ClientAddress = o.ClientAddress,
                    ClientEmail = o.ClientEmail,
                    ClientName = o.ClientName,
                    ClientPhoneNumber = o.ClientPhoneNumber,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalPrice = o.TotalPrice
                })
                .ToList();

            return new OrderQueryServiceModel
            {
                Orders = orders
            };
        }
    }
}
