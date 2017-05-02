using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.DAL.Repositories
{
    public class ExceptionDetailRepository : IRepository<ExceptionDetail>
    {
        public ApplicationContext Db { get; set; }

        public ExceptionDetailRepository(ApplicationContext context)
        {
            Db = context;
        }

        public IEnumerable<ExceptionDetail> GetAll()
            => Db.ExceptionDetails.Include(c => c.User);

        public ExceptionDetail Get(int id) => Db.ExceptionDetails.Find(id);

        public void Create(ExceptionDetail exceptionDetail) => Db.ExceptionDetails.Add(exceptionDetail);

        public void Update(ExceptionDetail exceptionDetail) => Db.Entry(exceptionDetail).State = EntityState.Modified;

        public IEnumerable<ExceptionDetail> Find(Func<ExceptionDetail, bool> predicate)
            => Db.ExceptionDetails.Where(predicate).ToList();

        public void Delete(int id)
        {
            ExceptionDetail exceptionDetail = Db.ExceptionDetails.Find(id);
            if (!ReferenceEquals(exceptionDetail, null))
                Db.ExceptionDetails.Remove(exceptionDetail);
        }
    }
}
