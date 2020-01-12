﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VivesRental.Model;

namespace VivesRental.WebApp.Models
{
    public class ProductViewModel : CommonViewModel
    {
        public IOrderedEnumerable<Product> Products { get; set; }
        public Product CurrentProduct { get; set; }
    }
}
