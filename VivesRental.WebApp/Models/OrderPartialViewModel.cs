using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VivesRental.Model;

namespace VivesRental.WebApp.Models
{
    public class OrderPartialViewModel
    {
        public Order Order { get; set; }
        public int Counter { get; set; }
    }
}