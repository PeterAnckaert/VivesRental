using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Results;

namespace VivesRental.WebApp.Models
{
    public class CustomerOrderPartialViewModel
    {
        public OrderResult Order { get; set; }
        public int Counter { get; set; }
        public IOrderedEnumerable<OrderLine> OrderLines { get; set; }
    }
}