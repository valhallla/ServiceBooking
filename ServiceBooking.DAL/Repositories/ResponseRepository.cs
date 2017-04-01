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
        public ApplicationContext Db { get; set; }

        public ResponseRepository(ApplicationContext context)
        {
            this.Db = context;
        }

        public IEnumerable<Response> GetAll()
        {
            return Db.Responses.Include(o => o.Order);
        }

        public Response Get(int id)
        {
            return Db.Responses.Find(id);
        }

        public void Create(Response book)
        {
            Db.Responses.Add(book);
        }

        public void Update(Response book)
        {
            Db.Entry(book).State = EntityState.Modified;
        }

        public IEnumerable<Response> Find(Func<Response, bool> predicate)
        {
            return Db.Responses.Include(o => o.Order).Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Response book = Db.Responses.Find(id);
            if (book != null)
                Db.Responses.Remove(book);
        }
    }
}
