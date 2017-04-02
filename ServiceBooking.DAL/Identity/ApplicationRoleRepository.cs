﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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