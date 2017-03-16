using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.DAL.Repositories
{
    public class ClientManager : IClientManager
    {
        public ApplicationContext Db { get; set; }

        public ClientManager(ApplicationContext db)
        {
            Db = db;
        }

        public void Create(ClientUser item)
        {
            Db.Clients.Add(item);
            Db.SaveChanges();
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
    }
}
