using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Controllers
{
    public class ProductController : ProductBaseController
    {
        public ProductController(RestaurantContext c) : base(c)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
     


    

      
        [HttpGet]
        public async Task<IActionResult> Index(char category)
        {
            var productCategory = await _context.products.Where(z => z.Category == category).ToListAsync();
            return View(productCategory);
        }

        [HttpGet]
        public async Task<IActionResult> ShowDetails( int id)
        {
            List<Product> product;
             product = await _context.products.Where(z => z.ProductId == id).ToListAsync();
        
         
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> ShowRandomProduct()
        {
            List<int> products;
   

            products = await _context.products.Select(z => z.ProductId).ToListAsync();

            Random r = new Random();

            var randomValues = Enumerable.Range(0, 2).Select(e => products[r.Next(products.Count)]);

            var items = await _context.products.Where(z => randomValues.Contains(z.ProductId)).ToListAsync();
            return Json(items);
        }


    }
}
