namespace Restaurant.Models
{
    public class BasketModel
    {

        public List<Basket> basket { get; set; }
        public decimal Total { get; internal set; }
    }
}
