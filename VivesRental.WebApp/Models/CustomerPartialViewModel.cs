using System;
using VivesRental.Model;

namespace VivesRental.WebApp.Models
{
    public class CustomerPartialViewModel
    {
        public Customer Customer { get; set; }
        public Guid CurrentCustomerId { get; set; }
    }
}
