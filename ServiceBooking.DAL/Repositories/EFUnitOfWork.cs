using System;
using System.Threading.Tasks;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Identity;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
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


        public EFUnitOfWork(string connectionString)
        {
            _db = new ApplicationContext(connectionString);
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

        public void Save()
        {
            _db.SaveChanges();
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

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
