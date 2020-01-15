using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using VivesRental.Services.Contracts;
using VivesRental.WebApp.Models;

namespace VivesRental.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService _articleService;
        private readonly ICustomerService _customerService;
        private readonly IOrderLineService _orderLineService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly HomeViewModel _homeViewModel = new HomeViewModel();

        public HomeController(ILogger<HomeController> logger, IArticleService articleService, ICustomerService customerService, IOrderLineService orderLineService, IOrderService orderService, IProductService productService)
        {
            _logger = logger;
            _articleService = articleService;
            _customerService = customerService;
            _orderLineService = orderLineService;
            _orderService = orderService;
            _productService = productService;
        }

        public IActionResult Index()
        {

            _homeViewModel.NbrArticles = _articleService.All().Count;
            _homeViewModel.NbrCustomers = _customerService.All().Count;
            _homeViewModel.NbrOrders = 0;
            foreach (var order in _orderService.All())
            {
                _homeViewModel.NbrOrders += _orderLineService.FindByOrderId(order.Id).Count;
            }
            _homeViewModel.NbrProducts = _productService.All().Count;
            return View(_homeViewModel);
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
