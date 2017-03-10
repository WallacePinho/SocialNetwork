using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using SocialNetwork.Api.App_Start;
using SocialNetwork.Api.Models;
using SocialNetwork.Api.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.Api {
    public partial class Startup {
        public static OAuthAuthorizationServerOptions oAuthOptions { get; private set; }

        public void ConfigureAuth(IAppBuilder app) {
            //Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            //Configure the application for OAuth based flow
            oAuthOptions = new OAuthAuthorizationServerOptions {
                Provider = new ApplicationOAuthProvider(),
                TokenEndpointPath = new PathString("/Token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1)
                };

            //Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(oAuthOptions);
        }
    }
}