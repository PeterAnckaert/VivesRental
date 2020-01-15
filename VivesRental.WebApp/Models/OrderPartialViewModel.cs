using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VivesRental.Model;
using VivesRental.Repository.Results;

namespace VivesRental.WebApp.Models
{
    public class OrderPartialViewModel
    {
        public OrderResult Order { get; set; }
        public int Counter { get; set; }
        public IOrderedEnumerable<OrderLine> OrderLines { get; set; }
    }
}