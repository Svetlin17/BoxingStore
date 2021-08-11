namespace BoxingStore.Controllers
{
    using AutoMapper;
    using BoxingStore.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using BoxingStore.Models.Orders;
    using BoxingStore.Services.Orders;
    using BoxingStore.Services.CartService;

    public class OrdersController : Controller
    {
        private readonly IOrderService orders;
        private readonly ICartService carts;
        private readonly BoxingStoreDbContext data;

        public OrdersController(IOrderService orders, ICartService carts, BoxingStoreDbContext data)
        {
            this.orders = orders;
            this.carts = carts;
            this.data = data;
        }

        public IActionResult Index([FromQuery] AllOrdersQueryModel query)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userIsAdmin = this.User.IsInRole("Administrator");

            var queryResult = this.orders.All(userId, userIsAdmin);

            query.Orders = queryResult.Orders;

            return this.View(query);
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

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult Info(int id)
        {
            var orderData = this.data.Orders.Find(id); //TODO put in service

            var orderProducts = this.orders.GetOrderProductsForOrder(id);

            return View(new OrderFormServiceModel 
            {
                IsCompleated = orderData.IsCompleated,
                ClientAddress = orderData.ClientAddress,
                ClientEmail = orderData.ClientEmail,
                ClientName = orderData.ClientName,
                ClientPhoneNumber = orderData.ClientPhoneNumber,
                OrderProducts = orderProducts,
                TotalPrice = orderData.TotalPrice
            });
        }

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Compleate(int id)
        {
            this.orders.CompleateOrder(id);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            this.orders.DeleteOrder(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
