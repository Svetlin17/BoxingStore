namespace BoxingStore.Models.Cart
{
    using System.Collections.Generic;

    public class CartViewModel
    {
        public int Id { get; set; }

        public double TotalPrice { get; set; }

        public IEnumerable<CartProductsQueryModel> CartProducts { get; set; }
    }
}
