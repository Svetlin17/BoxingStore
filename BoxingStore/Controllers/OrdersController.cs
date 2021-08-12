namespace BoxingStore.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using BoxingStore.Models.Orders;
    using BoxingStore.Services.Orders;
    using BoxingStore.Services.Carts;
    using BoxingStore.Services.User;
    using BoxingStore.Infrastructure;

    public class OrdersController : Controller
    {
        private readonly IOrderService orders;
        private readonly ICartService carts;
        private readonly IUserService users;

        public OrdersController(IOrderService orders, ICartService carts, IUserService users)
        {
            this.orders = orders;
            this.carts = carts;
            this.users = users;
        }

        [Authorize]
        public IActionResult Index([FromQuery] AllOrdersQueryModel query)
        {
            var userId = this.User.Id();
            var userIsAdmin = this.User.IsAdmin();

            var queryResult = this.orders.All(userId, userIsAdmin);

            query.Orders = queryResult.Orders;

            return this.View(query);
        }

        [Authorize]
        public IActionResult Create()
        {
            var userId = this.User.Id();

            return this.View(new OrderFormServiceModel
            {
                ClientName = this.users.FindByFullName(userId), 
                ClientEmail = this.users.FindByEmail(userId),
                TotalPrice = this.carts.GetCartTotalPrice(this.carts.GetUserCart(userId).Id)
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(OrderFormServiceModel orderModel)
        {
            var currentUserId = this.User.Id();

            var currentUserCart = this.carts.GetUserCart(currentUserId);

            var orderId = this.orders.Create(orderModel, currentUserId, currentUserCart.Id);

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult Info(int id)
        {
            var orderData = this.orders.FindById(id);

            var orderProducts = this.orders.GetOrderProductsForOrder(id);

            return View(new OrderFormServiceModel 
            {
                IsCompleated = orderData.IsCompleated,
                ClientAddress = orderData.ClientAddress,
                ClientEmail = orderData.ClientEmail,
                ClientName = orderData.ClientName,
                ClientPhoneNumber = orderData.ClientPhoneNumber,
                OrderProducts = orderProducts,
                TotalPrice = orderData.TotalPrice,
                OrderDate = orderData.OrderDate
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
