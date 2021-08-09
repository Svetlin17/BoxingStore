namespace BoxingStore.Controllers
{
    using AutoMapper;
    using BoxingStore.Data;
    using BoxingStore.Data.Models;
    using BoxingStore.Data.Models.Enums;
    using BoxingStore.Models.Products;
    using BoxingStore.Services.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Linq;
    using BoxingStore.Models.Orders;
    using BoxingStore.Services.Orders;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using BoxingStore.Services.CartService;

    public class OrdersController : Controller
    {
        private readonly IOrderService orders;
        private readonly ICartService carts;
        private readonly BoxingStoreDbContext data;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orders, ICartService carts, BoxingStoreDbContext data, IMapper mapper)
        {
            this.orders = orders;
            this.carts = carts;
            this.data = data;
            this.mapper = mapper;
        }

        public IActionResult Index([FromQuery] AllOrdersQueryModel query)
        {
            var queryResult = this.orders.All();

            query.Orders = queryResult.Orders;

            return View(query);
        }

        [Authorize]
        public IActionResult Create()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return this.View(new OrderFormServiceModel
            {
                ClientName = this.data.Users.Find(userId).FullName, //TODO make IUserService and UserService
                ClientEmail = this.data.Users.Find(userId).Email,
                TotalPrice = this.carts.GetCartTotalPrice(this.carts.GetUserCart(userId).Id)
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(OrderFormServiceModel orderModel)
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentUserCart = this.carts.GetUserCart(currentUserId);

            var orderId = this.orders.Create(orderModel, currentUserId, currentUserCart.Id);

            return RedirectToAction(nameof(Info), orderId);
        }

        [Authorize]
        public IActionResult Info(int id)
        {
            var orderData = this.data.Orders.Find(id); //this.orders.FindById(orderId);

            return View(new OrderFormServiceModel 
            {
                ClientAddress = orderData.ClientAddress,
                ClientEmail = orderData.ClientEmail,
                ClientName = orderData.ClientName,
                ClientPhoneNumber = orderData.ClientPhoneNumber,
                OrderProducts = orderData.OrderProducts,
                TotalPrice = orderData.TotalPrice
            });
        }
    }
}
