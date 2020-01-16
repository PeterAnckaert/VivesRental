using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;

namespace VivesRental.WebApp.Models
{
    public class CheckInViewModel
    {
        public CheckInViewModel()
        {
            SelectedArticles = new List<Article>();
        }

        public IOrderedEnumerable<Customer> Customers { get; set; }
        public IOrderedEnumerable<Article> Articles { get; set; }
        public Customer SelectedCustomer { get; set; }
        public IList<Article> SelectedArticles { get; set; }
    }
}