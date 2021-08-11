namespace BoxingStore.Services.Orders
{
    using BoxingStore.Data;
    using BoxingStore.Data.Models;
    using BoxingStore.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class OrderService : IOrderService
    {
        private readonly BoxingStoreDbContext data;

        public OrderService(BoxingStoreDbContext data)
        {
            this.data = data;
        }

        public int Create(OrderFormServiceModel order, string userId, int cartId)
        {
            var orderData = new Order //create order
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

            //we can take the id of the just created object
            CreateOrderProductsAndDeleteCartProducts(orderData.Id, cartId); // and delete the products from the cart
            RemovedOrderedProductSizeQuantites(orderData.Id);  //after the order is created, reduce quantities of the products in DB 

            orderData.TotalPrice = GetOrderTotalPrice(orderData.Id);

            this.data.SaveChanges();

            //TODO call IOrderService to remove from ProductSizeQuantities the ordered quantities

            return orderData.Id;
        }

        //public OrderInfoServiceModel FindById(int id)
        //    => this.data
        //    .Orders
        //    .Where(p => p.Id == id)
        //    .Select(o => new OrderInfoServiceModel
        //    {
        //        ClientAddress = o.ClientAddress,
        //        ClientEmail = o.ClientEmail,
        //        ClientName = o.ClientName,
        //        ClientPhoneNumber = o.ClientPhoneNumber,
        //        OrderProducts = o.OrderProducts,
        //        TotalPrice = o.TotalPrice
        //    })
        //    .FirstOrDefault();

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

            var orderProducts = this.data.OrderProducts.Where(x => x.OrderId == orderId).ToList();

            foreach (var orderProduct in orderProducts)
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

        private void CreateOrderProductsAndDeleteCartProducts(int orderId, int cartId)
        {
            var cartProducts = this.data.CartProducts.Where(c => c.CartId == cartId);

            foreach (var cartProduct in cartProducts)
            {
                var orderProduct = new OrderProduct //for every cart product creates an order product - analogically
                {
                    OrderId = orderId,
                    ProductId = cartProduct.ProductId,
                    Quantity = cartProduct.Quantity,
                    Size = cartProduct.Size
                };

                this.data.OrderProducts.Add(orderProduct);
                this.data.CartProducts.Remove(cartProduct);
            }
            this.data.SaveChanges();
        }
        private void RemovedOrderedProductSizeQuantites(int orderId)
        {
            var orderProducts = this.data.OrderProducts.Where(x => x.OrderId == orderId).ToList();

            foreach (var orderProduct in orderProducts)
            {
                var psq = this.data.ProductSizeQuantities.Where(p => p.ProductId == orderProduct.ProductId && p.Size == orderProduct.Size).FirstOrDefault();

                psq.Quantity -= orderProduct.Quantity;
            }
            this.data.SaveChanges();
        }
    }
}
