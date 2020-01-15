using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VivesRental.Model;
using VivesRental.Repository.Results;

namespace VivesRental.WebApp.Models
{
    public class CustomerViewModel : CommonViewModel
    {
        public IOrderedEnumerable<Customer> Customers { get; set; }
        public Customer CurrentCustomer { get; set; }
        public IOrderedEnumerable<OrderResult> CurrentCustomerOrders { get; set; }
        public List<IOrderedEnumerable<OrderLine>> CurrentCustomerOrderLines { get; set; }
    }
}
