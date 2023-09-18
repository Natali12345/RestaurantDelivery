namespace Restaurant.Models
{
    public class NewOrderModel
    {
        public class OrderTotalModel
        {
            public decimal Total { get; set; }
            public Order Order { get; internal set; }
        }
    }
}
