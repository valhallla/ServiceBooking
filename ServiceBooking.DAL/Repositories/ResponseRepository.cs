using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
            Db = context;
        }

        public IEnumerable<Response> GetAll() => Db.Responses.Include(o => o.Order);

        public Response Get(int id) => Db.Responses.Find(id);

        public void Create(Response response) => Db.Responses.Add(response);

        public void Update(Response response) => Db.Entry(response).State = EntityState.Modified;

        public IEnumerable<Response> Find(Func<Response, bool> predicate)
            => Db.Responses.Include(o => o.Order).Where(predicate).ToList();

        public void Delete(int id)
        {
            Response response = Db.Responses.Find(id);
            if (!ReferenceEquals(response, null))
                Db.Responses.Remove(response);
        }
    }
}
