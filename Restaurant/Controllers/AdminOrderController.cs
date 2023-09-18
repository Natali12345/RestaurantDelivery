using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using System.Data;

namespace Restaurant.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminOrderController : Controller
    {
        protected readonly RestaurantContext _context;


        public AdminOrderController(RestaurantContext c)
        {
            _context = c;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _context.orders.Include("OrderItems")
               .Include("OrderItems.Product")
               .ToListAsync();

            var model = new ListOfOrderTotalModels();
            foreach (var order in orders)
            {
                var total = order.OrderItems.Sum(x => x.TotalPrice);
                model.Add(new OrderTotalModel { Total = total, Order = order });
            }


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(string action, int id)
        {
            var status = action;
            var order = await _context.orders.FirstOrDefaultAsync(z => z.OrderId == id);

            order.Status = status;

            await _context.SaveChangesAsync();

            return RedirectToAction("AccessDenied", "Account");
        }



    }
}
