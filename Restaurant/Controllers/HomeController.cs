using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Controllers
{
    public class HomeController : Controller
    {
        protected readonly RestaurantContext _context;
        public HomeController(RestaurantContext c)
        {

            _context = c;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Product> product;
             product = await _context.products.ToListAsync();
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(int productId)
        {
            var image = await _context.productImages.FirstOrDefaultAsync(z => z.Product.ProductId == productId);
            return File(image.Bytes, "image/png");
        }
    }
}
