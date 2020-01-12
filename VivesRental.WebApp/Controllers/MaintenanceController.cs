﻿using Microsoft.AspNetCore.Mvc;
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
        private static ArticleViewModel _articleViewModel = new ArticleViewModel();

        public MaintenanceController(ILogger<MaintenanceController> logger, IArticleService articleService, ICustomerService customerService, IOrderLineService orderLineService, IOrderService orderService, IProductService productService)
        {
            _logger = logger;
            _articleService = articleService;
            _customerService = customerService;
            _orderLineService = orderLineService;
            _orderService = orderService;
            _productService = productService;
        }

        //IS NOG NIET VOLLEDIG JUIST. GEEN BERICHT BIJ 2 FOUTEN NA ELKAAR
        protected void ProcessError(CommonViewModel viewModel)
        {
            if (viewModel == null)
            {
                return;
            }

            if (viewModel.Error == null)
            {
                viewModel.Error = String.Empty;
            }

            if (viewModel.Error != String.Empty && viewModel.IsErrorShown)
            {
                viewModel.IsErrorShown = false;
                viewModel.Error = String.Empty;
            }

            if (viewModel.Error != String.Empty && !viewModel.IsErrorShown)
            {
                viewModel.IsErrorShown = true;
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Articles()
        {
            var articles = _articleService.All(new ArticleIncludes { Product = true });
            _articleViewModel.Products = _productService.All().OrderBy(ok => ok.Name);

            switch (_articleViewModel.SortKey)
            {
                case SortKey.NameAsc:
                    _articleViewModel.Articles = articles.OrderBy(ok => ok.Product.Name);
                    break;
                case SortKey.NameDesc:
                    _articleViewModel.Articles = articles.OrderByDescending(ok => ok.Product.Name);
                    break;
                case SortKey.DescriptionAsc:
                    _articleViewModel.Articles = articles.OrderBy(ok => ok.Product.Description);
                    break;
                case SortKey.DescriptionDesc:
                    _articleViewModel.Articles = articles.OrderByDescending(ok => ok.Product.Description);
                    break;
                case SortKey.ArticleStatusAsc:
                    _articleViewModel.Articles = articles.OrderBy(ok => ok.Status.ToString());
                    break;
                case SortKey.ArticleStatusDesc:
                    _articleViewModel.Articles = articles.OrderByDescending(ok => ok.Status.ToString());
                    break;
                case SortKey.ExpiresAtAsc:
                    _articleViewModel.Articles = articles.OrderBy(ok => ok.Product.RentalExpiresAfterDays);
                    break;
                case SortKey.ExpiresAtDesc:
                    _articleViewModel.Articles = articles.OrderByDescending(ok => ok.Product.RentalExpiresAfterDays);
                    break;
                default:
                    _articleViewModel.Articles = articles.OrderBy(ok => ok.Product.Name);
                    break;
            }

            ProcessError(_articleViewModel);

            return View(_articleViewModel);
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

            ProcessError(_productViewModel);

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

            ProcessError(_customerViewModel);

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
                if (_customerService.Remove(_customerViewModel.CurrentCustomer.Id) == false)
                {
                    _customerViewModel.Error = "Het verwijderen van de klant is mislukt";
                }
                else
                {
                    _customerViewModel.CurrentCustomer = null;
                }
            }
            else
            {
                _customerViewModel.Error = "Geen klant geselecteerd om te verwijderen";
            }
            return RedirectToAction("Customers");

        }

        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            if (customer == null)
            {
                _customerViewModel.Error = "Kan geen lege klant aanmaken";
                return RedirectToAction("Customers");
            }

            if (customer.FirstName == null || customer.LastName == null
                || customer.FirstName.Trim().Equals("<EMPTY>")
                || customer.LastName.Trim().Equals("<EMPTY>")
                || customer.FirstName.Trim().Length == 0
                || customer.LastName.Trim().Length == 0)
            {
                _customerViewModel.Error = "Voornaam en/of achternaam mogen niet leeg zijn";

            }
            else
            {
                if (_customerService.Create(customer) == null)
                {
                    _customerViewModel.Error = "Aanmaken van nieuwe klant is mislukt";
                }
                else
                {
                    _customerViewModel.CurrentCustomer = null;
                }
            }

            return RedirectToAction("Customers");
        }

        [HttpPost]
        public IActionResult UpdateCustomer(Customer customer)
        {
            if (customer.Id != Guid.Empty)
            {
                if (_customerService.Edit(customer) == null)
                {
                    _customerViewModel.Error = "Aanpassen van de klantgegevens is mislukt";
                }
                _customerViewModel.CurrentCustomer = null;
            }
            else
            {
                _customerViewModel.Error = "Aanpassen van de klantgegevens is mislukt omdat de id leeg was";
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
            else
            {
                _customerViewModel.Error = "Geen klant geselecteerd. Kan geen details tonen";
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
                if (_productService.Remove(_productViewModel.CurrentProduct.Id) == false)
                {
                    _productViewModel.Error = "Het verwijderen van het product is mislukt";
                }
                else
                {
                    _productViewModel.CurrentProduct = null;
                }
            }
            else
            {
                _productViewModel.Error = "Geen product geselecteerd om te verwijderen";
            }
            return RedirectToAction("Products");

        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (product == null)
            {
                _productViewModel.Error = "Kan geen leeg product aanmaken";
                return RedirectToAction("Customers");
            }
            if (product.Name ==null 
                || product.Name.Trim().Equals("<EMPTY>") 
                || product.Name.Trim().Length == 0)
            {
                _productViewModel.Error = "Naam van het product mag niet leeg zijn";
            }
            else
            {
                if (_productService.Create(product) == null)
                {
                    _productViewModel.Error = "Aanmaken van nieuwe klant is mislukt";
                }
                else
                {
                    _productViewModel.CurrentProduct = null;
                }
            }

            return RedirectToAction("Products");
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            if (product.Id != Guid.Empty)
            {
                if (_productService.Edit(product) == null)
                {
                    _productViewModel.Error = "Aanpassen van de produktgegevens is mislukt";
                }
                else
                {
                    _productViewModel.CurrentProduct = null;
                }
            }
            else
            {
                _customerViewModel.Error = "Aanpassen van de produktgegevens is mislukt omdat de id leeg was";
            }
            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult ProductDetail(Guid id)
        {
            if (id == Guid.Empty)
            {
                _productViewModel.Error = "Geen produkt geselecteerd. Kan geen details tonen";
            }
            var product = _productService.Get(id,new ProductIncludes{Articles=true});
            if (product != null)
            {
                return View(product);
            }
            else
            {
                _productViewModel.Error = String.Format("Produkt met id = {0} niet gevonden. Kan geen details tonen", id.ToString());
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

        [HttpGet]
        public IActionResult SetCurrentArticle(Article currentArticle)
        {
            _articleViewModel.CurrentArticle = currentArticle;
            return RedirectToAction("Articles");
        }

        [HttpPost]
        public IActionResult DeleteArticle()
        {
            if (_articleViewModel.CurrentArticle != null)
            {
                if (_articleService.Remove(_articleViewModel.CurrentArticle.Id) == false)
                {
                    _articleViewModel.Error = "Het verwijderen van het artikel is mislukt";
                }
                else
                {
                    _articleViewModel.CurrentArticle = null;
                }
            }
            else
            {
                _articleViewModel.Error = "Geen artikel geselecteerd om te verwijderen";
            }
            return RedirectToAction("Articles");

        }

        [HttpPost]
        public IActionResult CreateArticle(Article article)
        {
            if (article == null)
            {
                _articleViewModel.Error = "Kan geen leeg artikel aanmaken";
                return RedirectToAction("Articles");
            }

            if (article.ProductId == Guid.Empty)
            {
                _articleViewModel.Error = "Er moet een product gekozen worden";

            }
            else
            {
                if (_articleService.Create(article) == null)
                {
                    _articleViewModel.Error = "Aanmaken van nieuw artikel is mislukt";
                }
                else
                {
                    _articleViewModel.CurrentArticle = null;
                }
            }

            return RedirectToAction("Articles");
        }

        [HttpPost]
        public IActionResult UpdateArticle(Article article)
        {
            if (article.Id != Guid.Empty)
            {
                if (_articleService.UpdateStatus(article.Id,article.Status) == null)
                {
                    _articleViewModel.Error = "Aanpassen van de artikelgegevens is mislukt";
                }
                _articleViewModel.CurrentArticle = null;
            }
            else
            {
                _articleViewModel.Error = "Aanpassen van de artikelgegevens is mislukt omdat de id leeg was";
            }
            return RedirectToAction("Articles");
        }

        [HttpGet]
        public IActionResult ArticleDetail()
        {
            if (_articleViewModel.CurrentArticle != null)
            {
                return View(_articleViewModel.CurrentArticle);
            }
            else
            {
                _articleViewModel.Error = "Geen artikel geselecteerd. Kan geen details tonen";
            }
            return RedirectToAction("Articles");
        }

        [HttpGet]
        public IActionResult ClearCurrentArticle()
        {
            _articleViewModel.CurrentArticle = null;
            return RedirectToAction("Articles");
        }

        [HttpGet]
        public IActionResult SortArticles(SortKey sortKey)
        {
            _articleViewModel.SortKey = sortKey;
            return RedirectToAction("Articles");
        }
    }
}