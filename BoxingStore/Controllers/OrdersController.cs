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
    using BoxingStore.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OrdersController : Controller
    {
        private readonly IOrderService orders;
        private readonly BoxingStoreDbContext data;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orders
            , BoxingStoreDbContext data, IMapper mapper)
        {
            this.orders = orders;
            this.data = data;
            this.mapper = mapper;
        }

        public IActionResult All([FromQuery] AllOrdersQueryModel query)
        {
            var queryResult = this.orders.All();

            query.Orders = queryResult.Orders;

            return View(query);
        }

        public IActionResult Create(int id)
        {
            var cart = this.data.Carts.Find(id); //TODO move to a cart or order service

            var orderProducts = new List<OrderProduct>();

            var order = new Order();

            foreach (var item in cart.CartProducts)
            {
                //orderProducts.Add(new  { });
            }
            //query.Orders = queryResult.Orders;

            return View();
        }
    }
}
