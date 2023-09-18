using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant.Models;
using Restaurant.Services;
using System.Collections.Generic;
using System.Net;

namespace Restaurant.Controllers
{
    public class BasketController : Controller
    {

        protected readonly RestaurantContext _context;
        private readonly BasketHelper _basketHelper;

        public BasketController(RestaurantContext c, BasketHelper basketHelper)
        {

            _context = c;
            _basketHelper = basketHelper;

        }
        [HttpPost]
        public async Task<IActionResult> AddToBasket(int id, int availablen)
        {
           

            var identityUser = User.Identity.Name;

            var cookie = "";


            var item = await _context.baskets.FirstOrDefaultAsync(z => 
                (z.User.UserEmail== identityUser || (z.User == null &&  z.Cookie== cookie) ) && z.Product.ProductId == id);

            var product = await _context.products.FirstOrDefaultAsync(z => z.ProductId == id);
            if (item is null)
            {
                product.Availablen--;
                            var user = await _context.users.FirstOrDefaultAsync(z => z.UserEmail == identityUser);
                var totalItem = product.Price;
                var totalPrice = totalItem;
                var basket = new Basket
                {
                    Product = product,
                    Quantity = 1,
                    PricePerItem = totalItem,
                    TotalPrice = totalPrice,
                    User = user,
                    Cookie = cookie
                };

                _context.baskets.Add(basket);
            }
            else
            {
                product.Availablen--;
                item.Quantity++;
                item.TotalPrice = item.PricePerItem * item.Quantity;
               
            }
            _context.SaveChanges();

            return await LoadBasket();

        }



        public async Task<IActionResult> LoadBasket()
        {
            var model = await _basketHelper.LoadBasket(User.Identity.Name);
            return PartialView("ShowBasket", model); //make sure youhave view 
        }



        [HttpPost]
        public async Task<IActionResult> DeleteBasketProduct(int id)
        {

            //var productId = await _context.products.Map(z => z.ProductId == id).ToListAsync();
            var basket = await _context.baskets.Include("Product").FirstOrDefaultAsync(z => z.BasketId == id);
       
            if (basket != null)
            {


                basket.Product.Availablen++; //this might work if you include product in query, otherwise need load product like in add
                _context.baskets.Remove(basket);

            }
           
          

            _context.SaveChanges();

            return await LoadBasket();


        }


        [HttpPost]
        public async Task<IActionResult> ChangeQuantity(int id,string action)
        {
            var basket = await _context.baskets.Include("Product").FirstOrDefaultAsync(z => z.BasketId == id);
       
            if (action == "plus")
            {
              
                basket.Quantity++;
                basket.Product.Availablen--;
            }
            else if (action == "min")
            {
                if(basket.Quantity > 1)
                {

                    basket.Quantity--;
                    basket.Product.Availablen++;
                }
               
                else
                {
                    _context.baskets.Remove(basket);
                }
            }     
            basket.TotalPrice = basket.Quantity * basket.PricePerItem;
       
            _context.SaveChanges();
      
            return await LoadBasket();
        }




    }
}

