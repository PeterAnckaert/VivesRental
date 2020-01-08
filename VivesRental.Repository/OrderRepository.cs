﻿using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
using VivesRental.Repository.Mappers;
using VivesRental.Repository.Results;

namespace VivesRental.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IVivesRentalDbContext _context;

        public OrderRepository(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public Order Get(Guid id)
        {
            var query = _context.Orders
                .AsQueryable();
            return query.SingleOrDefault(o => o.Id == id);
        }
        
        public IEnumerable<Order> GetAll()
        {
            return _context.Orders
                .AsEnumerable();
        }

        public IEnumerable<OrderResult> GetAllResult()
        {
            return _context.Orders
                .MapToResults()
                .AsEnumerable();
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }

    }
}
