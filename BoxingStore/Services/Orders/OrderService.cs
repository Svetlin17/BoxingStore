﻿namespace BoxingStore.Services.Orders
{
    using AutoMapper;
    using BoxingStore;
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

            CreateOrderProductsForOrder(orderData.Id, cartId);
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

        public OrderQueryServiceModel All(string userId, bool isAdmin)
        {
            var ordersQuery = this.data.Orders.AsQueryable();

            if (!isAdmin)
            {
                ordersQuery = ordersQuery.Where(o => o.UserId == userId);
            }

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

        public IEnumerable<OrderProductsQueryModel> GetOrderProductsForOrder(int orderId)
        {
            var orderProducts = new List<OrderProductsQueryModel>();

            foreach (var orderProduct in this.data.OrderProducts.Where(x => x.OrderId == orderId).ToList())
            {
                var product = this.data.Products.Find(orderProduct.ProductId);

                orderProducts.Add(new OrderProductsQueryModel
                {
                    Id = orderProduct.Id,
                    Quantity = orderProduct.Quantity,
                    Size = orderProduct.Size,
                    ProductImageUrl = product.ImageUrl,
                    ProductName = product.Brand + " " + product.Name,
                    ProductId = product.Id,
                    Price = product.Price,
                    ProductTotalPrice = product.Price * orderProduct.Quantity //the totalprice of 1 single product in the cart
                });
            }

            return orderProducts;
        }

        private void CreateOrderProductsForOrder(int orderId, int cartId)
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
        }

        public void CompleateOrder(int id)
        {
            var orderData = this.data.Orders.Find(id);

            orderData.IsCompleated = true;

            this.data.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            var orderData = this.data.Orders.Find(id);

            if (orderData.IsCompleated)
            {
                this.data.Orders.Remove(orderData);

                this.data.SaveChanges();
            }
        }
    }
}
