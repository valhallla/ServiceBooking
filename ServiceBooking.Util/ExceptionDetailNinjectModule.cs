using Ninject.Modules;
using Ninject.Web.Common;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.BLL.Services;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.DAL.Repositories;

namespace ServiceBooking.Util
{
    public class ExceptionDetailNinjectModule : NinjectModule
    {
        public override void Load()
        {
            var connectionName = "DefaultConnection";
            var context = new ApplicationContext(connectionName);

            Bind<ApplicationContext>().ToConstructor(_ => new ApplicationContext(connectionName)).InRequestScope();

            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<UnitOfWork>().ToConstructor(_ => new UnitOfWork(context)).InRequestScope();

            Bind<IRepository<ExceptionDetail>>().To<ExceptionDetailRepository>();
            Bind<ExceptionDetailRepository>()
                .ToConstructor(_ => new ExceptionDetailRepository(context))
                .InRequestScope();
            Bind<IExceptionDetailService>().To<ExceptionDetailService>();

            Bind<IManyToManyResolver>().To<ClientRepository>();
            Bind<IRepository<ApplicationUser>>().To<ClientRepository>();
            Bind<ClientRepository>().ToConstructor(_ => new ClientRepository(context)).InRequestScope();
            Bind<IUserService>().To<UserService>();
        }
    }
}
