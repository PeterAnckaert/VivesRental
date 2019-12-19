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
        private readonly TestViewModel _tvm = new TestViewModel();

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
            _tvm.nbrArticles = _articleService.All().Count;
            _tvm.nbrCustomers = _customerService.All().Count;
//            _tvm.nbrOrders = _orderService.All().Count;
            _tvm.nbrProducts = _productService.All().Count;
            return View(_tvm);
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
