using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Data.Entity;
using SocialNetwork.Api.Models;

[assembly: OwinStartup(typeof(SocialNetwork.Api.Startup))]

namespace SocialNetwork.Api {
    public partial class Startup {

        public void Configuration(IAppBuilder app) {
            var config = new HttpConfiguration();

            ConfigureAuth(app);

            WebApiConfig.Register(config);
            app.UseWebApi(config);

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
        }
    }
}