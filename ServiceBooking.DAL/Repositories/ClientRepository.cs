using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.DAL.Repositories
{
    public class ClientRepository : IRepository<ApplicationUser>, IManyToManyResolver
    {
        public ApplicationContext Db { get; set; }

        public ClientRepository(ApplicationContext db)
        {
            Db = db;
        }

        public void Create(ApplicationUser item)
        {
            Db.Users.Add(item);
        }

        public void Delete(ApplicationUser item)
        {
            Db.Users.Remove(item);
            Db.SaveChanges();
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return Db.Users.Include(u => u.Categories).Include(o => o.Comments).Include(o => o.Orders);
        }

        public ApplicationUser Get(int id)
        {
            return Db.Users.Find(id);
        }

        public IEnumerable<ApplicationUser> Find(Func<ApplicationUser, bool> predicate)
        {
            return Db.Users.Where(predicate).ToList();
        }

        public void Update(ApplicationUser item)
        {
            Db.Entry(item).State = EntityState.Modified;
            Db.SaveChanges();
        }

        public void Delete(int id)
        {
            ApplicationUser client = Db.Users.Find(id);
            if (client != null)
                Db.Users.Remove(client);
            Db.SaveChanges();
        }

        public void Update(int id, int[] selectedItems)
        {
            var user = Get(id);

            if (user.Categories == null)
                user.Categories = new List<Category>();

            if (selectedItems == null)
            {
                foreach (var category in Db.Categories)
                {
                    if(user.Categories.Contains(category))
                        user.Categories.Remove(category);
                }
            }
            else
            {
                foreach (var category in Db.Categories.Where(c => !selectedItems.Contains(c.Id)))
                {
                    if (user.Categories.Contains(category))
                        user.Categories.Remove(category);   
                }
                foreach (var category in Db.Categories.Where(c => selectedItems.Contains(c.Id)))
                {
                    if (!user.Categories.Contains(category))
                        user.Categories.Add(category);
                }
            }

            Update(user);
        }
    }
}
