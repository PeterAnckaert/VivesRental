using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VivesRental.Model;

namespace VivesRental.WebApp.Models
{
    public class HomeViewModel
    {
        public int NbrArticles { get; set; }
        public int NbrCustomers { get; set; }
        public int NbrOrders { get; set; }
        public int NbrProducts { get; set; }
    }
}
