using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Controllers
{
    public class UserController : Controller
    {

        private readonly RestaurantContext _context;
        private readonly IConfiguration _configuration; 
        public UserController(RestaurantContext c, IConfiguration config)
        {

            _context = c;
            _configuration = config;

        }
        public IActionResult ShowLogin()
        {
            return View();
        }



        public IActionResult ShowRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegistration(RegistrationModel model)
        {


            var cheakUsers = await _context.users.FirstOrDefaultAsync(z => z.UserEmail == model.email) ;
            if (cheakUsers == null)
            {
                if (model.password == model.password2)
                {

                    byte[] salt = new byte[128 / 8];
                    using (RandomNumberGenerator rnd = RandomNumberGenerator.Create())
                        rnd.GetBytes(salt);


                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: model.password,
                        salt: salt,
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 100000,
                        numBytesRequested: 256 / 8));
                    hashed += "~";
                    hashed += Convert.ToBase64String(salt);

                    _context.users.Add(new User()
                    {
                        UserEmail = model.email,
                        UserPassword = hashed
                    });
                    _context.SaveChanges();
                    return RedirectToAction("ShowLogin");
                }

                else
                {
                    TempData["LoginError"] = "password no nice";
                    return RedirectToAction("ShowRegistration");
                }
            }
            else
            {
                TempData["LoginError"] = " phone already used";
                return RedirectToAction("ShowRegistration");

            }
        }

        [HttpPost]
        public async Task<IActionResult> GoToAccount(LoginModel model)
        {
            var user = _context.users.FirstOrDefault(z => z.UserEmail== model.email);
            if (user == null)
            {
                TempData["Error"] = "no correct password or email";
                return RedirectToAction("ShowLogin");

            }

            string[] records = user.UserPassword.Split('~'); 
            byte[] salt = Convert.FromBase64String(records[1]);
            string passwordHash = records[0];

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: model.password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            if (hashed == passwordHash)
            {
            

                List<Claim> claims = new();
                claims.Add(new Claim("sub", model.email));
                claims.Add(new Claim(ClaimTypes.Name, model.email));

                string adminAccount = _configuration["AdminAccount"]; 

                var isAdmin = user.UserEmail == adminAccount; 
                if (isAdmin)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "admin"));

                }
               

                await HttpContext.SignInAsync(
                    new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity(
                         claims, CookieAuthenticationDefaults.AuthenticationScheme)), new AuthenticationProperties()
                         {

                         });

                //if (isAdmin)
                //    return RedirectToAction("Index", "AdminProduct");
                //else

                    return RedirectToAction("Index", "Account");



            }
            else
            {
                TempData["Error"] = "no correct password or email";
                return RedirectToAction("ShowLogin");


            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }



}

