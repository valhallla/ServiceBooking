using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Interfaces;
using System.Data.Entity;

namespace ServiceBooking.DAL.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly ApplicationContext _db;

        public CategoryRepository(ApplicationContext context)
        {
            this._db = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return _db.Categories;
        }

        public Category Get(int id)
        {
            return _db.Categories.Find(id);
        }

        public void Create(Category book)
        {
            _db.Categories.Add(book);
        }

        public void Update(Category book)
        {
            _db.Entry(book).State = EntityState.Modified;
        }

        public IEnumerable<Category> Find(Func<Category, bool> predicate)
        {
            return _db.Categories.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Category book = _db.Categories.Find(id);
            if (book != null)
                _db.Categories.Remove(book);
        }
    }
}
