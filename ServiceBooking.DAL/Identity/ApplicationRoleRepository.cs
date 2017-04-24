using ServiceBooking.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ServiceBooking.DAL.Identity
{
    public class ApplicationRoleRepository : RoleManager<ApplicationRole>
    {
        public ApplicationRoleRepository(RoleStore<ApplicationRole> store)
                    : base(store)
        { }
    }
}
