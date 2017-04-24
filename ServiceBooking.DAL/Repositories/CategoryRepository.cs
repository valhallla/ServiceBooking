using System;
using System.Collections.Generic;
using System.Linq;
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
            Db = context;
        }

        public IEnumerable<Category> GetAll() 
            => Db.Categories.Include(c => c.Orders).Include(c => c.ApplicationUsers);

        public Category Get(int id) => Db.Categories.Find(id);

        public void Create(Category category) => Db.Categories.Add(category);

        public void Update(Category category) => Db.Entry(category).State = EntityState.Modified;

        public IEnumerable<Category> Find(Func<Category, bool> predicate)
            => Db.Categories.Where(predicate).ToList();

        public void Delete(int id)
        {
            Category category = Db.Categories.Find(id);
            if (!ReferenceEquals(category, null))
                Db.Categories.Remove(category);
        }
    }
}
