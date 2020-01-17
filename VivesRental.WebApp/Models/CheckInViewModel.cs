using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;

namespace VivesRental.WebApp.Models
{
    public class CheckInViewModel
    {
        public IOrderedEnumerable<Customer> Customers { get; set; }
        public IOrderedEnumerable<Article> RentedArticles { get; set; }
        public List<Article> CustomerArticles { get; set; }
        public Customer SelectedCustomer { get; set; }
        public string Error { get; set; }
        public bool KnownCustomer { get; set; }
    }
}