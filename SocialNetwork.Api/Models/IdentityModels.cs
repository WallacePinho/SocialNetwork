using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SocialNetwork.Api.Models {
    public class ApplicationUser : IdentityUser {
        public ApplicationUserProfile Profile { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType) {
            return await manager.CreateIdentityAsync(this, authenticationType);
        }
    }

    public class ApplicationUserProfile {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>{

        public ApplicationDbContext() : base ("DefaultConnection"){

        }
        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }

        public DbSet<ApplicationUserProfile> ApplicationUserProfiles { get; set; }
    }
}