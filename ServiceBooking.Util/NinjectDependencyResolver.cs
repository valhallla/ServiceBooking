using System;
using System.Collections.Generic;
using Ninject;
using System.Web.Mvc;
using Ninject.Web.Common;
using ServiceBooking.BLL;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.BLL.Services;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
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
            var context = new ApplicationContext(connectionName);

            _kernel.Bind<ApplicationContext>().ToConstructor(_ => new ApplicationContext(connectionName)).InRequestScope();

            _kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            _kernel.Bind<UnitOfWork>().ToConstructor(_ => new UnitOfWork(context)).InRequestScope();

            _kernel.Bind<IManyToManyResolver>().To<ClientRepository>();
            _kernel.Bind<IRepository<ApplicationUser>>().To<ClientRepository>();
            _kernel.Bind<ClientRepository>().ToConstructor(_ => new ClientRepository(context)).InRequestScope();
            _kernel.Bind<IUserService>().To<UserService>();

            _kernel.Bind<IRepository<Order>>().To<OrderRepository>();
            _kernel.Bind<OrderRepository>().ToConstructor(_ => new OrderRepository(context)).InRequestScope();
            _kernel.Bind<IOrderService>().To<OrderService>();

            _kernel.Bind<IRepository<Response>>().To<ResponseRepository>();
            _kernel.Bind<ResponseRepository>().ToConstructor(_ => new ResponseRepository(context)).InRequestScope();
            _kernel.Bind<IResponseService>().To<ResponseService>();

            _kernel.Bind<IRepository<Comment>>().To<CommentRepository>();
            _kernel.Bind<CommentRepository>().ToConstructor(_ => new CommentRepository(context)).InRequestScope();
            _kernel.Bind<ICommentService>().To<CommentService>();

            _kernel.Bind<IRepository<Category>>().To<CategoryRepository>();
            _kernel.Bind<CategoryRepository>().ToConstructor(_ => new CategoryRepository(context)).InRequestScope();
            _kernel.Bind<ICategoryService>().To<CategoryService>();

            _kernel.Bind<IRepository<Status>>().To<StatusRepository>();
            _kernel.Bind<StatusRepository>().ToConstructor(_ => new StatusRepository(context)).InRequestScope();
            _kernel.Bind<IStatusService>().To<StatusService>();

            _kernel.Bind<IRepository<Picture>>().To<PictureRepository>();
            _kernel.Bind<PictureRepository>().ToConstructor(_ => new PictureRepository(context)).InRequestScope();
            _kernel.Bind<IPictureService>().To<PictureService>();
        }
    }
}
