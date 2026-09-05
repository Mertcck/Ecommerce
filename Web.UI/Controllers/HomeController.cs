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
        public HomeController(ILogger<HomeController> logger, IHomeService service)
        {
            _logger = logger;
            this.service = service;
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
    }
}
