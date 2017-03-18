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
    public class ResponseRepository : IRepository<Response>
    {
        private ApplicationContext db;

        public ResponseRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public IEnumerable<Response> GetAll()
        {
            return db.Responses.Include(o => o.Order);
        }

        public Response Get(int id)
        {
            return db.Responses.Find(id);
        }

        public void Create(Response book)
        {
            db.Responses.Add(book);
        }

        public void Update(Response book)
        {
            db.Entry(book).State = EntityState.Modified;
        }

        public IEnumerable<Response> Find(Func<Response, bool> predicate)
        {
            return db.Responses.Include(o => o.Order).Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Response book = db.Responses.Find(id);
            if (book != null)
                db.Responses.Remove(book);
        }
    }
}
