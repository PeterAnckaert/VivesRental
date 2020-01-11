using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Includes;
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
        private static ProductViewModel _productViewModel = new ProductViewModel();

        public MaintenanceController(ILogger<MaintenanceController> logger, IArticleService articleService, ICustomerService customerService, IOrderLineService orderLineService, IOrderService orderService, IProductService productService)
        {
            _logger = logger;
            _articleService = articleService;
            _customerService = customerService;
            _orderLineService = orderLineService;
            _orderService = orderService;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Articles()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Products()
        {
            var products = _productService.All(new ProductIncludes { Articles = true });
            switch (_productViewModel.SortKey)
            {
                case SortKey.NameAsc:
                    _productViewModel.Products = products.OrderBy(ok => ok.Name);
                    break;
                case SortKey.NameDesc:
                    _productViewModel.Products = products.OrderByDescending(ok => ok.Name);
                    break;
                case SortKey.DescriptionAsc:
                    _productViewModel.Products = products.OrderBy(ok => ok.Description);
                    break;
                case SortKey.DescriptionDesc:
                    _productViewModel.Products = products.OrderByDescending(ok => ok.Description);
                    break;
                case SortKey.ManufacturerAsc:
                    _productViewModel.Products = products.OrderBy(ok => ok.Manufacturer);
                    break;
                case SortKey.ManufacturerDesc:
                    _productViewModel.Products = products.OrderByDescending(ok => ok.Manufacturer);
                    break;
                case SortKey.PublisherAsc:
                    _productViewModel.Products = products.OrderBy(ok => ok.Publisher);
                    break;
                case SortKey.PublisherDesc:
                    _productViewModel.Products = products.OrderByDescending(ok => ok.Publisher);
                    break;
                default:
                    _productViewModel.Products = products.OrderBy(ok => ok.Name);
                    break;
            }
            return View(_productViewModel);
        }

        [HttpGet]
        public IActionResult Customers()
        {
            var customers = _customerService.All();

            switch (_customerViewModel.SortKey)
            {
                case SortKey.FirstNameAsc:
                    _customerViewModel.Customers = customers.OrderBy(ok => ok.FirstName);
                    break;
                case SortKey.FirstNameDesc:
                    _customerViewModel.Customers = customers.OrderByDescending(ok => ok.FirstName);
                    break;
                case SortKey.LastNameAsc:
                    _customerViewModel.Customers = customers.OrderBy(ok => ok.LastName);
                    break;
                case SortKey.LastNameDesc:
                    _customerViewModel.Customers = customers.OrderByDescending(ok => ok.LastName);
                    break;
                case SortKey.EmailAsc:
                    _customerViewModel.Customers = customers.OrderBy(ok => ok.Email);
                    break;
                case SortKey.EmailDesc:
                    _customerViewModel.Customers = customers.OrderByDescending(ok => ok.Email);
                    break;
                case SortKey.PhoneNumberAsc:
                    _customerViewModel.Customers = customers.OrderBy(ok => ok.PhoneNumber);
                    break;
                case SortKey.PhoneNumberDesc:
                    _customerViewModel.Customers = customers.OrderByDescending(ok => ok.PhoneNumber);
                    break;
                default:
                    _customerViewModel.Customers = customers.OrderBy(ok => ok.FirstName);
                    break;
            }
            return View(_customerViewModel);
        }

        [HttpGet]
        public IActionResult SetCurrentCustomer(Customer currentCustomer)
        {
            _customerViewModel.CurrentCustomer = currentCustomer;
            return RedirectToAction("Customers");
        }

        [HttpPost]
        public IActionResult DeleteCustomer()
        {
            if (_customerViewModel.CurrentCustomer != null)
            {
                _customerService.Remove(_customerViewModel.CurrentCustomer.Id);
            }

            _customerViewModel.CurrentCustomer = null;
            return RedirectToAction("Customers");

        }

        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            _customerService.Create(customer);
            _customerViewModel.CurrentCustomer = null;

            return RedirectToAction("Customers");
        }

        [HttpPost]
        public IActionResult UpdateCustomer(Customer customer)
        {
            if (customer.Id != Guid.Empty)
            {
                _customerService.Edit(customer);
                _customerViewModel.CurrentCustomer = null;
            }
            return RedirectToAction("Customers");
        }

        [HttpGet]
        public IActionResult CustomerDetail()
        {
            if (_customerViewModel.CurrentCustomer != null)
            {
                return View(_customerViewModel.CurrentCustomer);
            }
            return RedirectToAction("Customers");
        }

        [HttpGet]
        public IActionResult ClearCurrentCustomer()
        {
            _customerViewModel.CurrentCustomer = null;
            return RedirectToAction("Customers");
        }

        [HttpGet]
        public IActionResult SortCustomers(SortKey sortKey)
        {
            _customerViewModel.SortKey = sortKey;
            return RedirectToAction("Customers");
        }

        [HttpGet]
        public IActionResult SetCurrentProduct(Product currentProduct)
        {
            _productViewModel.CurrentProduct = currentProduct;
            return RedirectToAction("Products");
        }

        [HttpPost]
        public IActionResult DeleteProduct()
        {
            if (_productViewModel.CurrentProduct != null)
            {
                _productService.Remove(_productViewModel.CurrentProduct.Id);
            }

            _productViewModel.CurrentProduct = null;
            return RedirectToAction("Products");

        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            _productService.Create(product);
            _productViewModel.CurrentProduct = null;

            return RedirectToAction("Products");
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            if (product.Id != Guid.Empty)
            {
                _productService.Edit(product);
                _productViewModel.CurrentProduct = null;
            }
            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult ProductDetail(Guid id)
        {
            var product = _productService.Get(id,new ProductIncludes{Articles=true});
            if (product != null)
            {
                return View(product);
            }
            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult ClearCurrentProduct()
        {
            _productViewModel.CurrentProduct = null;
            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult SortProducts(SortKey sortKey)
        {
            _productViewModel.SortKey = sortKey;
            return RedirectToAction("Products");
        }
    }

}