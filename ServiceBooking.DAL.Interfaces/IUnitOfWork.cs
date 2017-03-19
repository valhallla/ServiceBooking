//using ServiceBooking.DAL.Identity;
using System;
using System.Threading.Tasks;
//using ServiceBooking.DAL.Repositories;

namespace ServiceBooking.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        Task SaveAsync();
    }
}