namespace Restaurant.Models
{
    public class OrderItemModel
    {
        public int productId { get; set; }
        public int quantity { get; set; }
        public decimal pricePerItem { get; set; }
        public decimal totalPrice { get; set; }

    }
}