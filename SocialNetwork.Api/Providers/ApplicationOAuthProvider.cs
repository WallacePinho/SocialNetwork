using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using SocialNetwork.Api.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using SocialNetwork.Api.Models;

namespace SocialNetwork.Api.Providers {
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider {

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context) {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context) {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            var user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null) {
                context.SetError("invalid_grant", "The username or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);

            context.Validated(ticket);
        }

        public static AuthenticationProperties CreateProperties(ApplicationUser user) {
            IDictionary<string, string> data = new Dictionary<string, string>() {
                {"user_id", user.Id },
                {"user_name", user.UserName },
                {"has_profile", user.HasProfile.ToString() }
            };

            return new AuthenticationProperties(data);
        }

        public override async Task TokenEndpoint(OAuthTokenEndpointContext context) {
            foreach(var pair in context.Properties.Dictionary) {
                context.AdditionalResponseParameters.Add(pair.Key, pair.Value);
            }
        }

    }
}