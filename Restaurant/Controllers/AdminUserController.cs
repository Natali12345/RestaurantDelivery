using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminUserController : Controller
    {
        protected readonly RestaurantContext _context;


        public AdminUserController(RestaurantContext c)
        {
            _context = c;
        }

            [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _context.users.ToListAsync();

            return View(users); ;
        }
    }
}
