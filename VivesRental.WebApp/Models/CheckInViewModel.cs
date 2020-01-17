using System.Linq;
using VivesRental.Model;

namespace VivesRental.WebApp.Models
{
    public class CheckInViewModel
    {
        public IOrderedEnumerable<Customer> Customers { get; set; }
        public IOrderedEnumerable<Article> Articles { get; set; }
        public Customer SelectedCustomer { get; set; }
    }
}