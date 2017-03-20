using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Identity;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private readonly ApplicationContext _db;
        private OrderRepository _orderRepository;
        private ResponseRepository _responseRepository;
        private CommentRepository _commentRepository;
        private StatusRepository _statusRepository;
        private CategoryRepository _categoryRepository;

        public OrderRepository OrderRepository { get; set; }
        public ResponseRepository ResponseRepository { get; set; }
        public CommentRepository CommentRepository { get; set; }
        public StatusRepository StatusRepository { get; set; }
        public CategoryRepository CategoryRepository { get; set; }

        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;
        private readonly ClientRepository _clientManager;

        public ApplicationUserManager UserManager => _userManager;
        public ClientRepository ClientManager => _clientManager;
        public ApplicationRoleManager RoleManager => _roleManager;

        [Inject]
        public UnitOfWork(string connectionString)
        {
            _db = new ApplicationContext(connectionString);
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
            _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db));
            _clientManager = new ClientRepository(_db);
        }

        public IRepository<Category> Categories
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new CategoryRepository(_db);
                return _categoryRepository;
            }
        }

        public IRepository<Status> Statuses
        {
            get
            {
                if (_statusRepository == null)
                    _statusRepository = new StatusRepository(_db);
                return _statusRepository;
            }
        }

        public IRepository<Response> Responses
        {
            get
            {
                if (_responseRepository == null)
                    _responseRepository = new ResponseRepository(_db);
                return _responseRepository;
            }
        }

        public IRepository<Comment> Comments
        {
            get
            {
                if (_commentRepository == null)
                    _commentRepository = new CommentRepository(_db);
                return _commentRepository;
            }
        }

        public IRepository<Order> Orders
        {
            get
            {
                if (_orderRepository == null)
                    _orderRepository = new OrderRepository(_db);
                return _orderRepository;
            }
        }

        public async void Save()
        {
            await _db.SaveChangesAsync();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
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
