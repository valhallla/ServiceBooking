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
    public class StatusRepository : IRepository<Status>
    {
        public ApplicationContext Db { get; set; }

        public StatusRepository(ApplicationContext context)
        {
            this.Db = context;
        }

        public IEnumerable<Status> GetAll()
        {
            return Db.Status;
        }

        public Status Get(int id)
        {
            return Db.Status.Find(id);
        }

        public void Create(Status book)
        {
            Db.Status.Add(book);
        }

        public void Update(Status book)
        {
            Db.Entry(book).State = EntityState.Modified;
        }

        public IEnumerable<Status> Find(Func<Status, bool> predicate)
        {
            return Db.Status.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Status book = Db.Status.Find(id);
            if (book != null)
                Db.Status.Remove(book);
        }
    }
}
