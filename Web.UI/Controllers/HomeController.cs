using Core.Abstracts.IServices;
using Core.Concretes.DTOs.Sales;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.UI.Models;

namespace Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService service;
        private readonly IShoppingService shoppingService;
        public HomeController(ILogger<HomeController> logger, IHomeService service, IShoppingService shoppingService)
        {
            _logger = logger;
            this.service = service;
            this.shoppingService = shoppingService;
        }

        public async Task<IActionResult> Index(ProductsFilterDto filter)
        {
            return View(await service.GetAllAsync(filter));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            if (!await shoppingService.AddToCartAsync(User, productId, quantity))
            {
                TempData["ErrorMessage"] = "Sepete ekleme başarısız, daha sonra tekrar deneyin!";
            }
            return RedirectToAction("index");
        }
    }
}
