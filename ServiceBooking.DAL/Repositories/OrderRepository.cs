using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.DAL.Repositories
{
    class OrderRepository : IRepository<Order>
    {
        private ApplicationContext db;

        public OrderRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public IEnumerable<Order> GetAll()
        {
            return db.Orders.Include(o => o.Category).Include(o => o.Status);
        }

        public Order Get(int id)
        {
            return db.Orders.Find(id);
        }

        public void Create(Order book)
        {
            db.Orders.Add(book);
        }

        public void Update(Order book)
        {
            db.Entry(book).State = EntityState.Modified;
        }

        public IEnumerable<Order> Find(Func<Order, bool> predicate)
        {
            return db.Orders.Include(o => o.Category).Include(o => o.Status).Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Order book = db.Orders.Find(id);
            if (book != null)
                db.Orders.Remove(book);
        }
    }
}
