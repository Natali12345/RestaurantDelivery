using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Restaurant
{
    //oh so make this context a new name like RestaurantContext, you never want your class name to be same as base or other
    public class RestaurantContext : DbContext
    {
        public RestaurantContext() {
          
        }  

        public DbSet<User> users { get; set; }

        public DbSet<UserAddress> userAddresses{ get; set; } 

        public DbSet<Product> products{ get; set; }

        public DbSet<ProductImage> productImages { get; set; }

        public DbSet<Order> orders { get; set; }

        public DbSet<OrderItem> orderItems { get; set; }

        public DbSet<Basket> baskets {get; set; }
      


        public RestaurantContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


        }

    }

    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public decimal? Bonuses { get; set; }

    }


    public class UserAddress
    {
        [Key]
        public int UserAdrdressId { get; set; }    
        public User User { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Flat { get; set; }
    }


    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Ingridients { get; set; }
        public int Weight { get; set; }
        public List<ProductImage> Images { get; set; }
        public int Availablen { get; set; }
        public char Category { get; set; }
        //w

    }

    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        public byte[] Bytes { get; set; }

        public Product Product { get; set; }
    }

    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public User User { get; set; }
        public string Name { get; set; }

        public string Street { get; set; }
        public string House { get; set; }
        public string Flat { get; set; }
        public string Phone { get; set; }
        public string? Descriprion { get; set; }

        public string? Status { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new(); //name should have s 
    }
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
        public decimal TotalPrice { get; set; }
    }
    public class Basket
    {
        [Key]
        public int BasketId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; } 
     
        public int Quantity { get; set; }
        public User? User { get; set; }
        public string? Cookie { get; set; }
        public decimal PricePerItem { get; set; }
        public decimal TotalPrice { get; set; }
    }


}







   