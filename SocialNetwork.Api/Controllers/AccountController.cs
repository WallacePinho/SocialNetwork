using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SocialNetwork.Api.App_Start;
using SocialNetwork.Api.Models;
using SocialNetwork.Api.Models.Account;
using SocialNetwork.Api.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialNetwork.Api.Controllers {
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager {
            get {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set {
                _userManager = value;
            }
        }

        // POST: api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);


            if (result.Succeeded) {
                await SendEmailConfirmation(model.Email);
            } else {
                return GetErrorResult(result);
            }
            return Ok();
        }

        // GET: api/Account/Confirm
        [AllowAnonymous]
        [Route("Confirm")]
        [HttpGet]
        public async Task<IHttpActionResult> Confirm(string token, string userId) {
            var result = await UserManager.ConfirmEmailAsync(userId, token);
            if (!result.Succeeded) {
                return GetErrorResult(result);
            }

            return Ok();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result) {
            if (result == null) {
                return InternalServerError();
            }

            if (!result.Succeeded) {
                if (result.Errors != null) {

                    foreach (string error in result.Errors) {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid) {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }
            return null;
        }

        private async Task SendEmailConfirmation(string email) {
            var user = await UserManager.FindByEmailAsync(email);
            string token = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            EmailSender emailSender = new EmailSender();
            await emailSender.SendEmailConfirmation(token, email, user.Id);
        }

        protected override void Dispose(bool disposing) {

            if (disposing && _userManager != null) {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
    }
}
