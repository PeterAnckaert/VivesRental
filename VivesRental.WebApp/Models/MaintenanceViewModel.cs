using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VivesRental.Model;

namespace VivesRental.WebApp.Models
{
    public class MaintenanceViewModel
    {
        public IList<Product> Products { get; set; }
        public IList<Article> Articles { get; set; }
    }
}
