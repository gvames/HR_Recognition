using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext context;
        private readonly ILogger<DutchRepository> logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void AddEntity(object model)
        {
            context.Add(model);
        }

        public IEnumerable<Order> getAllOrders()
        {
            logger.LogInformation($"GetAllOrders was called");
            return context.Orders
                .Include(p=>p.Items)
                .ThenInclude(i=>i.Product)
                .ToList();
        }

        public IEnumerable<Order> getAllOrders(bool includeItems)
        {
            if (includeItems)
            {
                logger.LogInformation($"GetAllOrders was called");
                return context.Orders
                    .Include(p => p.Items)
                    .ThenInclude(i => i.Product)
                    .ToList();
            }
            else
            {
                logger.LogInformation($"GetAllOrders was called");
                return context.Orders                 
                    .ToList();
            }
       
        }

        public IEnumerable<Order> getAllOrdersByUser(string username, bool includeItems)
        {
            if (includeItems)
            {
                logger.LogInformation($"GetAllOrders was called");
                return context.Orders
                    .Where(p=>p.User.UserName == username)
                    .Include(p => p.Items)
                    .ThenInclude(i => i.Product)
                    .ToList();
            }
            else
            {
                logger.LogInformation($"GetAllOrders was called");
                return context.Orders
                    .ToList();
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {

            try
            {
                logger.LogInformation("GetAllProducts was called");
                return context.Products.ToList();
            }
            catch (Exception ex)
            {

                logger.LogError($"Failed to get all products: {ex}");
                return null;
            }
      
        }

        public Order GetOrderById(int id)
        {
            return context.Orders
                .Include(p=>p.Items)    
                .ThenInclude(o=>o.Product)
                .Where(s => s.Id == id)
                .FirstOrDefault();
        }

        public Order GetOrderById(string username, int orderid)
        {
            return context.Orders
              .Include(p => p.Items)
              .ThenInclude(o => o.Product)
              .Where(s => s.Id == orderid && s.User.UserName==username)
              .FirstOrDefault();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return context.Products.Where(p => p.Category == category).ToList();
        }

        public bool SaveAll()
        {
            return context.SaveChanges() > 0;
        }
    }
}
