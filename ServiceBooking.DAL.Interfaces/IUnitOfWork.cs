//using ServiceBooking.DAL.Identity;
using System;

//using ServiceBooking.DAL.Repositories;

namespace ServiceBooking.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}