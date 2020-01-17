using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Includes;
using VivesRental.Services.Contracts;
using VivesRental.WebApp.Models;

namespace VivesRental.WebApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly IOrderLineService _orderLineService;
        private static readonly CheckOutViewModel CheckOutViewModel = new CheckOutViewModel();
        private static readonly CheckInViewModel CheckInViewModel = new CheckInViewModel();

        public ShopController(IArticleService articleService, ICustomerService customerService, IOrderService orderService, IOrderLineService orderLineService)
        {
            _articleService = articleService;
            _customerService = customerService;
            _orderService = orderService;
            _orderLineService = orderLineService;
        }

        [HttpGet]
        public IActionResult CheckIn()
        {
            CheckInViewModel.Customers = _customerService.All().OrderBy(c => c.LastName).ThenBy(c => c.FirstName);
            CheckInViewModel.Articles = _articleService.GetRentedArticles(new ArticleIncludes { Product = true }).OrderBy(a => a.Product.Name).ToList();
            return View(CheckInViewModel);
        }

        [HttpGet]
        public IActionResult CheckOut()
        {
            CheckOutViewModel.Customers = _customerService.All().OrderBy(c => c.LastName).ThenBy(c => c.FirstName);
            CheckOutViewModel.AvailableArticles = _articleService.GetAvailableArticles(new ArticleIncludes { Product = true }).OrderBy(a => a.Product.Name);
            return View(CheckOutViewModel);
        }

        [HttpPost]
        public IActionResult Rent()
        {
            var order = _orderService.Create(CheckOutViewModel.SelectedCustomer.Id);
            IList<Guid> articleIds = new List<Guid>();
            foreach (var article in CheckOutViewModel.SelectedArticles)
            {
                articleIds.Add(article.Id);
            }
            if(!_orderLineService.Rent(order.Id,articleIds))
            {
                CheckOutViewModel.Error = "Niet alle artikelen zijn uitgechecked";
            }
            CheckOutViewModel.SelectedCustomer = null;
            CheckOutViewModel.SelectedArticles = new List<Article>();
            return RedirectToAction("CheckOut");
        }

        [HttpPost]
        public IActionResult ReturnNoCustomer(Guid id)
        {
            var orders = _orderService.All();
            foreach (var order in orders)
            {
                var orderLines = _orderLineService.FindByOrderId(order.Id);
                foreach (var orderLine in orderLines)
                {
                    if (!orderLine.ReturnedAt.HasValue && orderLine.ArticleId == id)
                    {
                        if(!_orderLineService.Return(orderLine.Id, DateTime.Now))
                        {
                            CheckInViewModel.Error = $"Het artikel '{orderLine.ProductName} [{orderLine.ArticleId}]' kon niet worden ingechecked";
                        }
                        return RedirectToAction("CheckIn");
                    }
                }
            }
            return RedirectToAction("CheckIn");
        }

        [HttpPost]
        public IActionResult SelectCustomerCheckOut(Guid id)
        {
            CheckOutViewModel.SelectedCustomer = _customerService.Get(id);
            return RedirectToAction("CheckOut");
        }

        [HttpPost]
        public IActionResult SelectCustomerCheckIn(Guid id)
        {
            CheckInViewModel.SelectedCustomer = _customerService.Get(id);
            var orders = _orderService.FindByCustomerIdResult(CheckInViewModel.SelectedCustomer.Id)
                .Where(o => o.NumberOfOrderLines > 0 && !o.ReturnedAt.HasValue);
            foreach (var order in orders)
            {
                var orderLines = _orderLineService.FindByOrderId(order.Id);
       //         CheckInViewModel.Articles.Add(orderLine.);
            }
            return RedirectToAction("CheckIn");
        }

        [HttpPost]
        public IActionResult SelectArticle(Guid id)
        {
            var article = _articleService.Get(id, new ArticleIncludes { Product = true });
            CheckOutViewModel.SelectedArticles.Add(article);
            return RedirectToAction("CheckOut");
        }

        [HttpGet]
        public IActionResult RemoveSelectedArticle(Guid id)
        {
            /*
            var article = _articleService.Get(id, new ArticleIncludes { Product = true });
            CheckOutViewModel.SelectedArticles.Remove(article);
            return RedirectToAction("CheckOut");
            //BOVENSTAANDE CODE WERKT NIET, CODE HIERONDER WEL
            */
            foreach (var article in CheckOutViewModel.SelectedArticles)
            {
                if (article.Id.Equals(id))
                {
                    CheckOutViewModel.SelectedArticles.Remove(article);
                    break;
                }
            }
            return RedirectToAction("CheckOut");
        }
    }
}