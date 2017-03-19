using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

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