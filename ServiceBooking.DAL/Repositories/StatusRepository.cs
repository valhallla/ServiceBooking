using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
            Db = context;
        }

        public IEnumerable<Status> GetAll() => Db.Status;

        public Status Get(int id) => Db.Status.Find(id);

        public void Create(Status status) => Db.Status.Add(status);

        public void Update(Status status) => Db.Entry(status).State = EntityState.Modified;

        public IEnumerable<Status> Find(Func<Status, bool> predicate) => Db.Status.Where(predicate).ToList();

        public void Delete(int id)
        {
            Status status = Db.Status.Find(id);
            if (!ReferenceEquals(status, null))
                Db.Status.Remove(status);
        }
    }
}
