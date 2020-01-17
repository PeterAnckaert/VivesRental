using System;
using VivesRental.Model;

namespace VivesRental.WebApp.Models
{
    public class ProductPartialViewModel
    {
        public Product Product { get; set; }
        public Guid CurrentProductId { get; set; }
    }
}
