using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace AngularStore.Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public Address Address { get; set; }
    }
}
