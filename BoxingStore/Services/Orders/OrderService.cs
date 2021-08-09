namespace BoxingStore.Services.Orders
{
    using AutoMapper;
    using BoxingStore.Data;
    using BoxingStore.Data.Models;
    using BoxingStore.Models;
    using BoxingStore.Services.Products;
    using System.Collections.Generic;
    using System.Linq;

    public class OrderService : IOrderService
    {
        private readonly IProductService products;
        private readonly BoxingStoreDbContext data;
        private readonly IConfigurationProvider mapper;

        public OrderService(IProductService products, BoxingStoreDbContext data, IMapper mapper)
        {
            this.products = products;
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public int Create(OrderFormServiceModel order, string userId, int cartId)
        {
            var orderData = new Order
            {
                ClientAddress = order.ClientAddress,
                ClientEmail = order.ClientEmail,
                ClientName = order.ClientName,
                ClientPhoneNumber = order.ClientPhoneNumber,
                TotalPrice = order.TotalPrice,
                UserId = userId
            };

            this.data.Orders.Add(orderData);
            this.data.SaveChanges();

            orderData.OrderProducts = CreateOrderProductsForOrder(orderData.Id, cartId);
            orderData.TotalPrice = GetOrderTotalPrice(orderData.Id);
            this.data.SaveChanges();

            //TODO call IOrderService to remove from ProductSizeQuantities the ordered quantities

            return orderData.Id;
        }

        public OrderInfoServiceModel FindById(int id)
            => this.data
            .Orders
            .Where(p => p.Id == id)
            .Select(o => new OrderInfoServiceModel
            {
                ClientAddress = o.ClientAddress,
                ClientEmail = o.ClientEmail,
                ClientName = o.ClientName,
                ClientPhoneNumber = o.ClientPhoneNumber,
                OrderProducts = o.OrderProducts,
                TotalPrice = o.TotalPrice
            })
            .FirstOrDefault();

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
                    IsCompleated = o.IsCompleated,
                    TotalPrice = o.TotalPrice
                })
                .ToList();

            return new OrderQueryServiceModel
            {
                Orders = orders
            };
        }

        public double GetOrderTotalPrice(int orderId)
        {
            var orderTotalPrice = 0.0;

            foreach (var orderProduct in this.data.OrderProducts.Where(x => x.OrderId == orderId).ToList())
            {
                var product = this.data.Products.Find(orderProduct.ProductId);

                orderTotalPrice += product.Price * orderProduct.Quantity;
            }

            return orderTotalPrice;
        }

        private IEnumerable<OrderProduct> CreateOrderProductsForOrder(int orderId, int cartId)
        {
            foreach (var cartProduct in this.data.CartProducts.Where(c => c.CartId == cartId))
            {
                var orderProduct = new OrderProduct
                {
                    OrderId = orderId,
                    ProductId = cartProduct.ProductId,
                    Quantity = cartProduct.Quantity,
                    Size = cartProduct.Size
                };

                this.data.OrderProducts.Add(orderProduct);
                
            }
            this.data.SaveChanges();

            return this.data.OrderProducts
                .Where(o => o.OrderId == orderId)
                .ToList();
        }
    }
}
