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

        public IEnumerable<ApplicationUser> GetAll() 
            => Db.Users.Include(u => u.Categories).Include(o => o.Comments).Include(o => o.Orders);

        public ApplicationUser Get(int id) => Db.Users.Find(id);

        public void Create(ApplicationUser item) => Db.Users.Add(item);

        public IEnumerable<ApplicationUser> Find(Func<ApplicationUser, bool> predicate) 
            => Db.Users.Where(predicate).ToList();

        public void Update(ApplicationUser item) => Db.Entry(item).State = EntityState.Modified;

        public void Delete(int id)
        {
            ApplicationUser client = Db.Users.Find(id);
            if (!ReferenceEquals(client, null))
                Db.Users.Remove(client);
        }

        public void Update(int id, int[] selectedItems)
        {
            var user = Get(id);

            if (!ReferenceEquals(user.Categories, null))
                user.Categories = new List<Category>();

            if (ReferenceEquals(selectedItems, null))
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
