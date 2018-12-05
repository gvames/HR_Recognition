using System.Collections.Generic;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);

        bool SaveAll();
        IEnumerable<Order> getAllOrders(bool includeItems);
        IEnumerable<Order> getAllOrdersByUser(string username, bool includeItems);
        Order GetOrderById(int id);
        Order GetOrderById(string name, int orderid);
        void AddEntity(object model);
       
    }
}