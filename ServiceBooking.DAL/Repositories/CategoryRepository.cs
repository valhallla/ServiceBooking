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
        private ApplicationContext db;

        public CategoryRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return db.Categories;
        }

        public Category Get(int id)
        {
            return db.Categories.Find(id);
        }

        public void Create(Category book)
        {
            db.Categories.Add(book);
        }

        public void Update(Category book)
        {
            db.Entry(book).State = EntityState.Modified;
        }

        public IEnumerable<Category> Find(Func<Category, bool> predicate)
        {
            return db.Categories.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Category book = db.Categories.Find(id);
            if (book != null)
                db.Categories.Remove(book);
        }
    }
}
