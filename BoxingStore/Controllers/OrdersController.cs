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
    }
}
