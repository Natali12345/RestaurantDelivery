using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;

namespace Restaurant.Controllers
{
    [Authorize(Roles ="admin")] 
    public class AdminProductController : ProductBaseController
    {
        public AdminProductController(RestaurantContext c) : base(c) 
        {
        }
        

        [HttpGet]
        public async Task<IActionResult> Index() hmm
        {
            return View();
        }
      
        [HttpPost]
        public async Task<IActionResult> Index(AddNewProductModel model)
        {
            var imageData = new byte[model.image.Length];
            using (var ms = model.image.OpenReadStream())

            {
                ms.Read(imageData, 0, imageData.Length);
            }

            bool productCheak = (await _context.products.Where(z => z.ProductName == model.productName).FirstOrDefaultAsync()) != null;
            if (!productCheak)
            {
                var img = await _context.products.Include("Images").ToListAsync();
                _context.products.Add(new Product()
                {
                    ProductId = model.productId,
                    ProductName = model.productName,
                    Price = model.price,
                    Ingridients = model.ingridients,
                    Weight = model.weight,
                    Images = new List<ProductImage>()
                    {
                        new ProductImage()
                        {
                            Bytes = imageData
                        }
                    },
                    Category = model.category,
                    Availablen = model.available

                });

                await _context.SaveChangesAsync();

            }
        
            List<Product> values = await LoadProductData();
            return Json(values);



        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productId = await _context.products.FirstOrDefaultAsync(z => z.ProductId == id);

            if (productId != null)
            {
                _context.products.Remove(productId);
                await _context.SaveChangesAsync();
            }
            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> SaveChanges( ChangeModel model)
        {
            var stringError = "";
            var changedProd = await _context.products.FirstOrDefaultAsync(z => z.ProductId == model.Id);
            if (changedProd != null)
            {
               
                switch (model.Name)
                {
                   case "changeName":
                    
                        changedProd.ProductName = model.Value;
                   break;
                    case "changePrice":
                        if (decimal.TryParse(model.Value, out var priceValue))
                        {
                            changedProd.Price = priceValue;
                            
                        }
                        else
                        {
                            stringError = "Write number!";
                        }
                        break;
                    case "changeWeight":
                        if (int.TryParse(model.Value, out var weightValue))
                        {
                            changedProd.Weight = weightValue;
                           
                        }
                        else
                        {
                            stringError = "Write numbeer!";
                        }
                        break;
                    case "changeCategory":
                        if (char.TryParse(model.Value, out var categoryValue))
                        {
                            changedProd.Category = categoryValue;
                        
                        }
                        else
                        {
                            stringError = "Write char!";
                        }
                        break;
                    case "changeIngridients":                   
                            changedProd.Ingridients = model.Value;
                        break;




                }
                       
            }
            await _context.SaveChangesAsync();

            return Json(stringError); //i really dont know what put here ,, this better so far, but i would make logic that 
            //tell false if something no good, and a string saying what no good
            //so if dadsa insterad of decimal can have striung from else that tell use number idiot 
            //oh understand, ya , now in your ajax success stuff you can check this

        }

    }
}
