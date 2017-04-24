using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.DAL.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        public ApplicationContext Db { get; set; }

        public OrderRepository(ApplicationContext context)
        {
            Db = context;
        }

        public IEnumerable<Order> GetAll() => Db.Orders.Include(o => o.Responses);

        public Order Get(int id) => Db.Orders.Find(id);

        public void Create(Order order) => Db.Orders.Add(order);

        public void Update(Order order) => Db.Entry(order).State = EntityState.Modified;
    
        public IEnumerable<Order> Find(Func<Order, bool> predicate)
            => Db.Orders.Include(o => o.Category).Include(o => o.Status).Where(predicate).ToList();

        public void Delete(int id)
        {
            Order order = Db.Orders.Find(id);
            if (!ReferenceEquals(order, null))
                Db.Orders.Remove(order);
        }
    }
}
