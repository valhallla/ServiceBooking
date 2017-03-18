using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using ServiceBooking.DAL.Identity;
using Ninject;

namespace ServiceBooking.DAL.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _db;

        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;
        private readonly ClientManager _clientManager;

        [Inject]
        public IdentityUnitOfWork(string connectionString)
        {
            _db = new ApplicationContext(connectionString);
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
            _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db));
            _clientManager = new ClientManager(_db);
        }

        public ApplicationUserManager UserManager => _userManager;

        public ClientManager ClientManager => _clientManager;

        public ApplicationRoleManager RoleManager => _roleManager;

        public async void Save()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _userManager.Dispose();
                    _roleManager.Dispose();
                    _clientManager.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
