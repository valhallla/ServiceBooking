using System;
using System.Collections.Generic;
using Ninject;
using System.Web.Mvc;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.BLL.Services;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.DAL.Repositories;

namespace ServiceBooking.Util
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            _kernel = kernelParam;
            AddBindings();
        }

        //public static string ConnectionName { get; set; }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            var connectionName = "DefaultConnection";

            _kernel.Bind<IdentityUnitOfWork>().ToConstructor(_ => new IdentityUnitOfWork(connectionName));
            _kernel.Bind<IUserService>().To<UserService>();
            //_kernel.Bind<UserService>().ToConstructor(_ => new UserService(new IdentityUnitOfWork(connectionName)));
        }
    }
}
