using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.DAL.Repositories
{
    public class ClientManager : IRepository<ClientUser>
    {
        public ApplicationContext Db { get; set; }

        public ClientManager(ApplicationContext db)
        {
            Db = db;
        }

        public void Create(ClientUser item)
        {
            Db.Clients.Add(item);
            //Db.SaveChanges();
        }

        public void Delete(ClientUser item)
        {
            Db.Clients.Remove(item);
            Db.SaveChanges();
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public IEnumerable<ClientUser> GetAll()
        {
            return Db.Clients;
        }

        public ClientUser Get(int id)
        {
            return Db.Clients.Find(id);
        }

        public IEnumerable<ClientUser> Find(Func<ClientUser, bool> predicate)
        {
            return Db.Clients.Where(predicate).ToList();
        }

        public void Update(ClientUser item)
        {
            Db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            ClientUser client = Db.Clients.Find(id);
            if (client != null)
                Db.Clients.Remove(client);
        }
    }
}
