using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Controllers
{
    public class ProductBaseController : Controller
    {
        protected readonly RestaurantContext _context;
        public ProductBaseController(RestaurantContext c)
        {

            _context = c;

        }

        public async Task<IActionResult> Load()
        {
            var data = await _context.products.ToListAsync();
            return Json(data);
        }

        protected async Task<List<Product>> LoadProductData()
        {
           return  await _context.products.ToListAsync();
        }



        [HttpGet]
        public async Task<IActionResult> GetImage(int productId)
        {
            var image = await _context.productImages.FirstOrDefaultAsync(z => z.Product.ProductId == productId);
            return File(image.Bytes, "image/png");
        }


    }
}
