using ServiceBooking.DAL.Identity;
using System;
using ServiceBooking.DAL.Repositories;

namespace ServiceBooking.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        ClientManager ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }

        void Save();
    }
}