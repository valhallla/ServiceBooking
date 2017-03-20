using System;
using System.Collections.Generic;
using Ninject;
using System.Web.Mvc;
using Ninject.Web.Common;
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

            _kernel.Bind<UnitOfWork>().ToConstructor(_ => new UnitOfWork(connectionName)).InRequestScope();
            _kernel.Bind<IUserService>().To<UserService>();
            //_kernel.Bind<UserService>().ToConstructor(_ => new UserService(new IdentityUnitOfWork(connectionName)));
        }
    }
}
