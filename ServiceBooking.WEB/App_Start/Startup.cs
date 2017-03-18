using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.BLL.Services;
using ServiceBooking.Util;
using ServiceBooking.Util.App_Start;

[assembly: OwinStartup(typeof(ServiceBooking.WEB.Startup))]

namespace ServiceBooking.WEB
{
    public class Startup
    {
        //IServiceCreator serviceCreator = new ServiceCreator();
        public void Configuration(IAppBuilder app)
        {
            //app.CreatePerOwinContext(CreateUserService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
            //NinjectDependencyResolver.ConnectionName = "DefaultConnection";
            //NinjectWebCommon.ConnectionName = "DefaultConnection";
        }

        
    }
}