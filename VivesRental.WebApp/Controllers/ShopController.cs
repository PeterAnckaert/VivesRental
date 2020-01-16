using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult CheckIn()
        {
            CheckInViewModel.Articles = _articleService.GetRentedArticles(new ArticleIncludes { Product = true }).OrderBy(a => a.Product.Name);
            return View(CheckInViewModel);
        }
        public IActionResult CheckOut()
        {
            CheckOutViewModel.Customers = _customerService.All().OrderBy(c => c.LastName).ThenBy(c => c.FirstName);
            CheckOutViewModel.Articles = _articleService.GetAvailableArticles(new ArticleIncludes { Product = true }).OrderBy(a => a.Product.Name);
            return View(CheckOutViewModel);
        }

        public IActionResult Rent()
        {
            var order = _orderService.Create(CheckOutViewModel.SelectedCustomer.Id);
            IList<Guid> articleIds = new List<Guid>();
            foreach (var article in CheckOutViewModel.SelectedArticles)
            {
                articleIds.Add(article.Id);
            }
            _orderLineService.Rent(order.Id,articleIds);
            CheckOutViewModel.SelectedCustomer = null;
            CheckOutViewModel.SelectedArticles = new List<Article>();
            return RedirectToAction("CheckOut");
        }

        public IActionResult SelectCustomer(Guid id)
        {
            CheckOutViewModel.SelectedCustomer = _customerService.Get(id);
            return RedirectToAction("CheckOut");
        }
        
        public IActionResult SelectArticle(Guid id)
        {
            var article = _articleService.Get(id, new ArticleIncludes {Product = true});
            CheckOutViewModel.SelectedArticles.Add(article);
            return RedirectToAction("CheckOut");
        }

        public IActionResult RemoveSelectedArticle(Guid id)
        {
/*            var article = _articleService.Get(id, new ArticleIncludes { Product = true });
            CheckOutViewModel.SelectedArticles.Remove(article);
            return RedirectToAction("CheckOut");*/
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