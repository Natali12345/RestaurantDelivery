namespace Restaurant.Models
{
    public class OrderModel
    {


        public int orderId { get; set; }
        public string name { get; set; }
        public string street { get; set; }
        public string house { get; set; }

        public string flat { get; set; }
        public string phone { get; set; }
        public string description { get; set; }
    
        public List<OrderItemModel> items { get; set; }


       
    }
}
