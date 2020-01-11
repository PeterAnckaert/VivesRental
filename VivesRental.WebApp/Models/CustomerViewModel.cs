﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VivesRental.Model;

namespace VivesRental.WebApp.Models
{
    public class CustomerViewModel
    {
        public IOrderedEnumerable<Customer> Customers { get; set; }
        public Customer CurrentCustomer { get; set; }
        public SortKey SortKey { get; set; }
    }
}
