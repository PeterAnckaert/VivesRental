using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VivesRental.Model;

namespace VivesRental.WebApp.Models
{
    public class TestViewModel
    {
        public int nbrArticles { get; set; }
        public int nbrCustomers { get; set; }
        public int nbrOrders { get; set; }
        public int nbrProducts { get; set; }

        private IList<Article> Articles { get; set; }
    }
}
