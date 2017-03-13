using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Repositories;

namespace ServiceBooking.BLL.Services
{
    public class ServiceCreator : IServiceCreator//NINJECT!!!!!!!!!!!!!!!!!
    {
        public IUserService CreateUserService(string connection)
        {
            return new UserService(new IdentityUnitOfWork(connection));
        }
    }
}