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
        private readonly IArticleService _articleService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private static readonly CustomerViewModel CustomerViewModel = new CustomerViewModel();
        private static readonly ProductViewModel ProductViewModel = new ProductViewModel();
        private static readonly ArticleViewModel ArticleViewModel = new ArticleViewModel();

        public MaintenanceController(IArticleService articleService, ICustomerService customerService, IProductService productService)
        {
            _articleService = articleService;
            _customerService = customerService;
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
                viewModel.Error = string.Empty;
            }

            if (viewModel.Error != string.Empty && viewModel.IsErrorShown)
            {
                viewModel.IsErrorShown = false;
                viewModel.Error = string.Empty;
            }

            if (viewModel.Error != string.Empty && !viewModel.IsErrorShown)
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
            var articles = _articleService.All(new ArticleIncludes { Product = true, OrderLines = true });

            ArticleViewModel.Products = _productService.All().OrderBy(ok => ok.Name);

            ArticleViewModel.Articles = ArticleViewModel.SortKey switch
            {
                SortKey.NameAsc => articles.OrderBy(a => a.Product.Name),
                SortKey.NameDesc => articles.OrderByDescending(a => a.Product.Name),
                SortKey.DescriptionAsc => articles.OrderBy(a => a.Product.Description),
                SortKey.DescriptionDesc => articles.OrderByDescending(a => a.Product.Description),
                SortKey.ArticleStatusAsc => articles.OrderBy(a => a.Status.ToString()),
                SortKey.ArticleStatusDesc => articles.OrderByDescending(a => a.Status.ToString()),
                SortKey.ExpiresAtAsc => articles.OrderBy(a => a.Product.RentalExpiresAfterDays),
                SortKey.ExpiresAtDesc => articles.OrderByDescending(a => a.Product.RentalExpiresAfterDays),
                _ => articles.OrderBy(a => a.Product.Name),
            };

            ProcessError(ArticleViewModel);

            return View(ArticleViewModel);
        }

        [HttpGet]
        public IActionResult Products()
        {
            var products = _productService.All(new ProductIncludes { Articles = true });

            ProductViewModel.Products = ProductViewModel.SortKey switch
            {
                SortKey.NameAsc => products.OrderBy(p => p.Name),
                SortKey.NameDesc => products.OrderByDescending(p => p.Name),
                SortKey.DescriptionAsc => products.OrderBy(p => p.Description),
                SortKey.DescriptionDesc => products.OrderByDescending(p => p.Description),
                SortKey.ManufacturerAsc => products.OrderBy(p => p.Manufacturer),
                SortKey.ManufacturerDesc => products.OrderByDescending(p => p.Manufacturer),
                SortKey.PublisherAsc => products.OrderBy(p => p.Publisher),
                SortKey.PublisherDesc => products.OrderByDescending(p => p.Publisher),
                _ => products.OrderBy(p => p.Name),
            };
            ProcessError(ProductViewModel);

            return View(ProductViewModel);
        }

        [HttpGet]
        public IActionResult Customers()
        {
            var customers = _customerService.All();

            CustomerViewModel.Customers = CustomerViewModel.SortKey switch
            {
                SortKey.FirstNameAsc => customers.OrderBy(c => c.FirstName),
                SortKey.FirstNameDesc => customers.OrderByDescending(c => c.FirstName),
                SortKey.LastNameAsc => customers.OrderBy(c => c.LastName),
                SortKey.LastNameDesc => customers.OrderByDescending(c => c.LastName),
                SortKey.EmailAsc => customers.OrderBy(c => c.Email),
                SortKey.EmailDesc => customers.OrderByDescending(c => c.Email),
                SortKey.PhoneNumberAsc => customers.OrderBy(c => c.PhoneNumber),
                SortKey.PhoneNumberDesc => customers.OrderByDescending(c => c.PhoneNumber),
                _ => customers.OrderBy(c => c.FirstName),
            };

            ProcessError(CustomerViewModel);

            return View(CustomerViewModel);
        }

        [HttpGet]
        public IActionResult SetCurrentCustomer(Customer currentCustomer)
        {
            CustomerViewModel.CurrentCustomer = currentCustomer;
            return RedirectToAction("Customers");
        }

        [HttpPost]
        public IActionResult DeleteCustomer()
        {
            if (CustomerViewModel.CurrentCustomer != null)
            {
                if (_customerService.Remove(CustomerViewModel.CurrentCustomer.Id) == false)
                {
                    CustomerViewModel.Error = "Het verwijderen van de klant is mislukt";
                }
                else
                {
                    CustomerViewModel.CurrentCustomer = null;
                }
            }
            else
            {
                CustomerViewModel.Error = "Geen klant geselecteerd om te verwijderen";
            }
            return RedirectToAction("Customers");

        }

        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            if (customer == null)
            {
                CustomerViewModel.Error = "Kan geen lege klant aanmaken";
                return RedirectToAction("Customers");
            }
            if (customer.FirstName == null || customer.LastName == null
                || customer.FirstName.Trim().Equals("<EMPTY>")
                || customer.LastName.Trim().Equals("<EMPTY>")
                || customer.FirstName.Trim().Length == 0
                || customer.LastName.Trim().Length == 0)
            {
                CustomerViewModel.Error = "Voornaam en/of achternaam mogen niet leeg zijn";
            }
            else
            {
                if (_customerService.Create(customer) == null)
                {
                    CustomerViewModel.Error = "Aanmaken van nieuwe klant is mislukt";
                }
                else
                {
                    CustomerViewModel.CurrentCustomer = null;
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
                    CustomerViewModel.Error = "Aanpassen van de klantgegevens is mislukt";
                }
                CustomerViewModel.CurrentCustomer = null;
            }
            else
            {
                CustomerViewModel.Error = "Aanpassen van de klantgegevens is mislukt omdat de id leeg was";
            }
            return RedirectToAction("Customers");
        }

        [HttpGet]
        public IActionResult CustomerDetail()
        {
            if (CustomerViewModel.CurrentCustomer != null)
            {
                return View(CustomerViewModel.CurrentCustomer);
            }
            else
            {
                CustomerViewModel.Error = "Geen klant geselecteerd. Kan geen details tonen";
            }
            return RedirectToAction("Customers");
        }

        [HttpGet]
        public IActionResult ClearCurrentCustomer()
        {
            CustomerViewModel.CurrentCustomer = null;
            return RedirectToAction("Customers");
        }

        [HttpGet]
        public IActionResult SortCustomers(SortKey sortKey)
        {
            CustomerViewModel.SortKey = sortKey;
            return RedirectToAction("Customers");
        }

        [HttpGet]
        public IActionResult SetCurrentProduct(Product currentProduct)
        {
            ProductViewModel.CurrentProduct = currentProduct;
            return RedirectToAction("Products");
        }

        [HttpPost]
        public IActionResult DeleteProduct()
        {
            if (ProductViewModel.CurrentProduct != null)
            {
                if (_productService.Remove(ProductViewModel.CurrentProduct.Id) == false)
                {
                    ProductViewModel.Error = "Het verwijderen van het product is mislukt";
                }
                else
                {
                    ProductViewModel.CurrentProduct = null;
                }
            }
            else
            {
                ProductViewModel.Error = "Geen product geselecteerd om te verwijderen";
            }
            return RedirectToAction("Products");

        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (product == null)
            {
                ProductViewModel.Error = "Kan geen leeg product aanmaken";
                return RedirectToAction("Customers");
            }
            if (product.Name == null
                || product.Name.Trim().Equals("<EMPTY>")
                || product.Name.Trim().Length == 0)
            {
                ProductViewModel.Error = "Naam van het product mag niet leeg zijn";
            }
            else
            {
                if (_productService.Create(product) == null)
                {
                    ProductViewModel.Error = "Aanmaken van nieuwe klant is mislukt";
                }
                else
                {
                    ProductViewModel.CurrentProduct = null;
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
                    ProductViewModel.Error = "Aanpassen van de produktgegevens is mislukt";
                }
                else
                {
                    ProductViewModel.CurrentProduct = null;
                }
            }
            else
            {
                CustomerViewModel.Error = "Aanpassen van de produktgegevens is mislukt omdat de id leeg was";
            }
            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult ProductDetail(Guid id)
        {
            if (id == Guid.Empty)
            {
                ProductViewModel.Error = "Geen produkt geselecteerd. Kan geen details tonen";
            }
            var product = _productService.Get(id, new ProductIncludes { Articles = true });
            if (product != null)
            {
                return View(product);
            }
            else
            {
                ProductViewModel.Error = $"Produkt met id = {id.ToString()} niet gevonden. Kan geen details tonen";
            }
            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult ClearCurrentProduct()
        {
            ProductViewModel.CurrentProduct = null;
            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult SortProducts(SortKey sortKey)
        {
            ProductViewModel.SortKey = sortKey;
            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult SetCurrentArticle(Article currentArticle)
        {
            ArticleViewModel.CurrentArticle = currentArticle;
            return RedirectToAction("Articles");
        }

        [HttpPost]
        public IActionResult DeleteArticle()
        {
            if (ArticleViewModel.CurrentArticle != null)
            {
                if (_articleService.Remove(ArticleViewModel.CurrentArticle.Id) == false)
                {
                    ArticleViewModel.Error = "Het verwijderen van het artikel is mislukt";
                }
                else
                {
                    ArticleViewModel.CurrentArticle = null;
                }
            }
            else
            {
                ArticleViewModel.Error = "Geen artikel geselecteerd om te verwijderen";
            }
            return RedirectToAction("Articles");

        }

        [HttpPost]
        public IActionResult CreateArticle(Article article, int aantal)
        {
            if (article == null)
            {
                ArticleViewModel.Error = "Kan geen leeg artikel aanmaken";
                return RedirectToAction("Articles");
            }

            if (article.ProductId == Guid.Empty)
            {
                ArticleViewModel.Error = "Er moet een product gekozen worden";

            }
            else if (aantal <= 0)
            {
                ArticleViewModel.Error = "Er moet een aantal artikelen gekozen worden";
            }
            else
            {
                if (aantal == 1)
                {
                    if (_articleService.Create(article) == null)
                    {
                        ArticleViewModel.Error = "Aanmaken van het nieuwe artikel is mislukt";
                    }
                }
                else
                {
                    if (_productService.GenerateArticles(article.ProductId, aantal) == false)
                    {
                        ArticleViewModel.Error = "Aanmaken van nieuwe artikelen is mislukt";
                    }
                    else
                    {
                        if (article.Status != ArticleStatus.Normal)
                        {
                            ArticleViewModel.Error = "Status artikelen was niet 'Normal', artikelen zijn ingevoegd als normal";
                        }
                        ArticleViewModel.CurrentArticle = null;
                    }
                }
            }

            return RedirectToAction("Articles");
        }

        [HttpPost]
        public IActionResult UpdateArticle(Article article)
        {
            if (article.Id != Guid.Empty)
            {
                if (_articleService.UpdateStatus(article.Id, article.Status) == false)
                {
                    ArticleViewModel.Error = "Aanpassen van de artikelgegevens is mislukt";
                }
                ArticleViewModel.CurrentArticle = null;
            }
            else
            {
                ArticleViewModel.Error = "Aanpassen van de artikelgegevens is mislukt omdat de id leeg was";
            }
            return RedirectToAction("Articles");
        }

        [HttpGet]
        public IActionResult ArticleDetail()
        {
            if (ArticleViewModel.CurrentArticle != null)
            {
                return View(ArticleViewModel.CurrentArticle);
            }
            else
            {
                ArticleViewModel.Error = "Geen artikel geselecteerd. Kan geen details tonen";
            }
            return RedirectToAction("Articles");
        }

        [HttpGet]
        public IActionResult ClearCurrentArticle()
        {
            ArticleViewModel.CurrentArticle = null;
            return RedirectToAction("Articles");
        }

        [HttpGet]
        public IActionResult SortArticles(SortKey sortKey)
        {
            ArticleViewModel.SortKey = sortKey;
            return RedirectToAction("Articles");
        }
    }
}