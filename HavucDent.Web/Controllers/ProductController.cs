using HavucDent.Application.Interfaces;
using HavucDent.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HavucDent.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProductAsync(product);

                return RedirectToAction("Index");
            }
            return View(product); // Eğer model hatalıysa aynı sayfaya dön
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products); // Ürünleri listele
        }
    }
}
}
