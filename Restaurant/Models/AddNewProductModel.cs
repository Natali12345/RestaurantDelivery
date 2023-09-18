using Microsoft.AspNetCore.Http.HttpResults;

namespace Restaurant.Models
{
    public class AddNewProductModel
    {
        public int productId { get; set; }
        public string productName { get; set; }

        public decimal price { get; set; }
        public string ingridients { get; set; } 

        public int weight { get; set; }
        public IFormFile image { get; set; }
        public char category { get; set; }
        public char available { get; set; }
    }
}

