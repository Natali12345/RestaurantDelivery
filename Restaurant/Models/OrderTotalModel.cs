namespace Restaurant.Models
{
    public class OrderTotalModel
    {
        public decimal Total { get; set; }
        public Order Order { get; internal set; }


    }

    public class ListOfOrderTotalModels : List<OrderTotalModel>
    {
        public List<OrderTotalModel> OrderFilterModel(string status)
        {
            return this.Where(z => z.Order.Status == status).ToList();
        }
    }

}
