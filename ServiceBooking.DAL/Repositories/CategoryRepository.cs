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
        public ApplicationContext Db { get; set; }

        public CategoryRepository(ApplicationContext context)
        {
            this.Db = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return Db.Categories;
        }

        public Category Get(int id)
        {
            return Db.Categories.Find(id);
        }

        public void Create(Category book)
        {
            Db.Categories.Add(book);
        }

        public void Update(Category book)
        {
            Db.Entry(book).State = EntityState.Modified;
        }

        public IEnumerable<Category> Find(Func<Category, bool> predicate)
        {
            return Db.Categories.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Category book = Db.Categories.Find(id);
            if (book != null)
                Db.Categories.Remove(book);
        }
    }
}
