namespace BoxingStore.Models.Cart
{
    using System.Collections.Generic;

    public class CartViewModel
    {
        public int Id { get; set; }

        public ICollection<CartProductsQueryModel> CartProducts { get; set; }

        public double TotalPrice { get; set; }
    }
}
