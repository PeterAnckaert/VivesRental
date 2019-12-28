using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VivesRental.Model;
using VivesRental.Services.Contracts;
using VivesRental.WebApp.Models;

namespace VivesRental.WebApp.Controllers
{

    public class MaintenanceController : Controller
    {
        private readonly ILogger<MaintenanceController> _logger;
        private readonly IArticleService _articleService;
        private readonly ICustomerService _customerService;
        private readonly IOrderLineService _orderLineService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly MaintenanceViewModel _maintenanceViewModel = new MaintenanceViewModel();
        private static CustomerViewModel _customerViewModel = new CustomerViewModel();

        public MaintenanceController(ILogger<MaintenanceController> logger, IArticleService articleService, ICustomerService customerService, IOrderLineService orderLineService, IOrderService orderService, IProductService productService)
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
            return View();
        }
        public IActionResult Articles()
        {
            return View();
        }
        public IActionResult Products()
        {
            return View();
        }
        public IActionResult Customers()
        {
            _customerViewModel.Customers = _customerService.All();
            return View(_customerViewModel);
        }

        public IActionResult SetCurrentCustomer(Customer currentCustomer)
        {
            _customerViewModel.CurrentCustomer = currentCustomer;
            return RedirectToAction("Customers");
        }

        public IActionResult DeleteCustomer()
        {
            _customerService.Remove(_customerViewModel.CurrentCustomer.Id);
            return RedirectToAction("Customers");
        }

        public IActionResult CreateCustomer(Customer customer)
        {
            _customerService.Create(customer);
            return RedirectToAction("Customers");
        }
    }
}