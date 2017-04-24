using System;
using Ninject;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private readonly ApplicationContext _db;

        [Inject]
        public UnitOfWork(ApplicationContext context)
        {
            _db = context;
        }
        
        public void Save() => _db.SaveChanges();

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    _db.Dispose();
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
