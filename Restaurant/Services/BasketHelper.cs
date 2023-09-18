using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;

namespace Restaurant.Services
{
    public class BasketHelper
    {
        private readonly RestaurantContext _context;

        public BasketHelper(RestaurantContext context) {
            _context = context;
        }


    
        public async Task<BasketModel> LoadBasket(string userId)
        {


            var model = new BasketModel(); //you made this model for view but in view used List<Basket> yes indside model list
            //yes but we give view this, not list 
            
            var basketMeal = await _context.baskets.Where(z => z.User.UserEmail == userId)
                .Include("Product") //Include tell entity framework also read product like inner join
                                    //if we just reead basket table, this like select * from basket
                                    //include tell select from backet inner join product on ids

                .ToListAsync();


            model.Total = basketMeal.Sum(z => z.TotalPrice);
            model.basket = basketMeal;


            return model;


        }
    }
}
