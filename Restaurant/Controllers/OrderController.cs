using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using Restaurant.Services;
using System.Net;

namespace Restaurant.Controllers
{
    public class OrderController : Controller
    {

        protected readonly RestaurantContext _context;
        private readonly BasketHelper _basketHelper;

        public OrderController(RestaurantContext c, BasketHelper basketHelper)
        {

            _context = c;
            _basketHelper = basketHelper;

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderModel model)
        {





            //for items, you want to read the basket of the user from helper. not items from order model
            //yo same as other means User.Identity.Name

            var userName = User.Identity.Name;
            var user = await _context.users.FirstOrDefaultAsync(z => z.UserEmail == userName);
            var items = await _basketHelper.LoadBasket(userName);
            var status = "Getorder";
            var order = new Order()
            {
                Name = model.name,
                Street = model.street,
                House = model.house,
                Flat = model.flat,
                Phone = model.phone,
                Descriprion = model.description,
                Status = status,
                User = user

            };
          

            var userAdresses = await _context.userAddresses.FirstOrDefaultAsync(z => z.User == user);
            if (userAdresses == null)
            {
                var userAdress = new UserAddress()
                {
                    User = user,
                    Street = model.street,
                    House = model.house,
                    Flat = model.flat
                };
                _context.userAddresses.Add(userAdress);
            }
            else
            {


                if (userAdresses.Street != model.street || userAdresses.House != model.house || userAdresses.Flat != model.flat)
                {


                    var userAdress = new UserAddress()
                    {
                        User = user,
                        Street = model.street,
                        House = model.house,
                        Flat = model.flat
                    };
                    _context.userAddresses.Add(userAdress);
                }
            }

            foreach (var item in items.basket)
            {

                var basketProduct = await _context.products.FirstOrDefaultAsync(z => z.ProductId == item.ProductId);



                var orderItem = new OrderItem()
                {
                    Quantity = item.Quantity,

                    Product = basketProduct,
                    PricePerItem = item.PricePerItem,
                    TotalPrice = item.TotalPrice
                };

                var bonus = 0.0m;
                order.OrderItems.Add(orderItem);


             
                if(item.TotalPrice >= 8 && item.TotalPrice <= 14)
                {
                    bonus = item.TotalPrice / 100 * 10;
                }
                else if (item.TotalPrice >= 15 && item.TotalPrice <= 24)
                {
                    bonus = item.TotalPrice / 100 * 15;
                }
                else if (item.TotalPrice >= 25 )
                {
                    bonus = item.TotalPrice / 100 * 20;
                }
                else
                {
                    bonus = 1;
                }

                //var userBonus = new User()
                //{
                //    Bonuses = bonus
                //};
                //_context.users.Add(userBonus);
                if (user.Bonuses is null)
                    user.Bonuses = bonus;
                else
                    user.Bonuses += bonus;

            }


           
            _context.orders.Add(order);

            await _context.SaveChangesAsync();
            foreach (var item in items.basket)
            {
                _context.baskets.Remove(item);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Account");

        }
    }
}

