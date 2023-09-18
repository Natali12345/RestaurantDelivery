using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant;
using Restaurant.Models;

namespace Restaurant.Controllers
{
    public class AccountController : Controller
    {

        protected readonly RestaurantContext _context;


        public AccountController(RestaurantContext c)
        {
            _context = c;

        }

        [HttpGet]
        public async Task<IActionResult> Index(LoginModel model)
        {
            var user = User.Identity.Name;
            var addresses = await _context.userAddresses.Where(z => z.User.UserEmail == user).Include("User").ToListAsync();

            return View(addresses);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(ChangeModel model)
        {
            var stringError = "";
            var changedUserData = await _context.userAddresses.FirstOrDefaultAsync(z => z.User.UserId == model.Id);
            if (changedUserData != null)
            {
                switch (model.Name)
                {
                    case "changeEmail":
                        changedUserData.User.UserEmail = model.Value;
                        break;
                    case "changeStreet":
                        changedUserData.Street = model.Value;                   
                        break;
                    case "changeHouse":
                        changedUserData.House = model.Value;
                        break;
                    case "changeFlat":
                        changedUserData.Flat = model.Value;                      
                        break;
                }
            }
            await _context.SaveChangesAsync();
            return Json(stringError);

        }




        [HttpPost]
        public async Task<IActionResult> EditPassword(EditPasswordModel model)
        {
            var user = User.Identity.Name;
            var userPas = await _context.users.FirstOrDefaultAsync(z => z.UserEmail == user);

            string[] records = userPas.UserPassword.Split('~');
            byte[] salt = Convert.FromBase64String(records[1]);
            string passwordHash = records[0];

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: model.OldPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            if (hashed == records[0])
            {
          
                string newHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: model.NewPassword,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

            
                userPas.UserPassword = newHashed + "~" + Convert.ToBase64String(salt);
   
                await _context.SaveChangesAsync();
            }
            else
            {
           
                ModelState.AddModelError(string.Empty, "old password wrong");
            }

            return RedirectToAction("Index", "Account");
        }



        public IActionResult AccessDenied()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ShowAddresses()
        {
            var user = User.Identity.Name;
            var addresses = await _context.userAddresses.Where(z => z.User.UserEmail == user).ToListAsync();


            return View(addresses);


        }


        [HttpGet]
        public async Task<IActionResult> ShowOrders()
        {


            var user = User.Identity.Name;
            var orders = await _context.orders.Where(z => z.User.UserEmail == user)
               .Include("OrderItems")
               .Include("OrderItems.Product")
               .ToListAsync();

            var model = new List<OrderTotalModel>();
            foreach (var order in orders)
            {
                var total = order.OrderItems.Sum(x => x.TotalPrice);
                model.Add(new OrderTotalModel { Total = total, Order = order });
            }


            return View(model);
        }


    }
}

