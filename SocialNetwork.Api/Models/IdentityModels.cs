using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SocialNetwork.Api.Models {
    public class ApplicationUser : IdentityUser {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType) {
            return await manager.CreateIdentityAsync(this, authenticationType);
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>{

        public ApplicationDbContext() : base ("DefaultConnection"){

        }
        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }
    }
}