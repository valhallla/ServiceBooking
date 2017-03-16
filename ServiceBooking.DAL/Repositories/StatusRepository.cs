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
    class StatusRepository : IRepository<Status>
    {
        private ApplicationContext db;

        public StatusRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public IEnumerable<Status> GetAll()
        {
            return db.Status;
        }

        public Status Get(int id)
        {
            return db.Status.Find(id);
        }

        public void Create(Status book)
        {
            db.Status.Add(book);
        }

        public void Update(Status book)
        {
            db.Entry(book).State = EntityState.Modified;
        }

        public IEnumerable<Status> Find(Func<Status, bool> predicate)
        {
            return db.Status.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Status book = db.Status.Find(id);
            if (book != null)
                db.Status.Remove(book);
        }
    }
}
