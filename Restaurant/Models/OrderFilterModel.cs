namespace Restaurant.Models
{
    public class OrderFilterModel
    {
        public OrderFilterModel(List<OrderTotalModel> orders, string filter)
        {
            Orders = orders;
            Filter = filter;
        }

        public List<OrderTotalModel> Orders { get; }
        public string Filter { get; }
    }
}
