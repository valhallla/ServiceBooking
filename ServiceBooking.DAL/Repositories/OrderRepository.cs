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
    public class OrderRepository : IRepository<Order>
    {
        private readonly ApplicationContext _db;

        public OrderRepository(ApplicationContext context)
        {
            this._db = context;
        }

        public IEnumerable<Order> GetAll()
        {
            return _db.Orders.Include(o => o.Category).Include(o => o.Status);
        }

        public Order Get(int id)
        {
            return _db.Orders.Find(id);
        }

        public void Create(Order book)
        {
            _db.Orders.Add(book);
        }

        public void Update(Order book)
        {
            _db.Entry(book).State = EntityState.Modified;
        }

        public IEnumerable<Order> Find(Func<Order, bool> predicate)
        {
            return _db.Orders.Include(o => o.Category).Include(o => o.Status).Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Order book = _db.Orders.Find(id);
            if (book != null)
                _db.Orders.Remove(book);
        }
    }
}
