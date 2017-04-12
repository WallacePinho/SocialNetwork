using SocialNetwork.Api.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SocialNetwork.Api.Models;
using System.Threading.Tasks;
using System.Web.Http.Description;
using System.IO;
using SocialNetwork.Api.Models.Account;
using SocialNetwork.Api.Core.Azure;
using MimeTypes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SocialNetwork.Api.Controllers {

    [RoutePrefix("api/Profile")]
    public class ProfileController : ApiController {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager {
            get {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set {
                _userManager = value;
            }
        }

        private ApplicationDbContext _db;
        private ImageManager _imageManager;

        public ProfileController() {
            _db = new ApplicationDbContext();
            _imageManager = new ImageManager();
        }

        // POST: api/Profile?userId={userId}
        [HttpPost]
        public async Task<IHttpActionResult> Post(ProfileBindingModel profile, string userId) {
            try {

                var stream = new MemoryStream(profile.ProfilePicture.InputStream);

                var path = await WriteOnDisk(stream, profile.ProfilePicture.ContentType);
                var user = UserManager.FindById(userId);
                var profilePicUri = await _imageManager.UploadFileAsync(path);

                var prof = new ApplicationUserProfile() {
                    ProfilePicture = profilePicUri,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    BirthDate = profile.BirthDate,
                    UserName = profile.UserName
                };

                user.Profile = prof;
                user.HasProfile = true;
                var result = await UserManager.UpdateAsync(user);

                if (!result.Succeeded) {
                    return InternalServerError();
                }

                return Ok();
            } catch (Exception ex) {
                throw ex;
            }
        }

        // GET: api/Profile?userName={userName}
        [ResponseType(typeof(ApplicationUserProfile))]
        public async Task<IHttpActionResult> Get(string userName) {
            var profile = _db.ApplicationUserProfiles.SingleOrDefault(x => x.UserName.ToLower().Equals(userName.ToLower()));

            return Ok(profile);
        }

        private async Task<string> WriteOnDisk(Stream file, string mimeType) {
            //Create tmp folder
            var tmpFolder = AppDomain.CurrentDomain.BaseDirectory + @"tmp\";
            Directory.CreateDirectory(tmpFolder);
            //Write file
            var path = tmpFolder + DateTime.Now.Ticks + MimeTypeMap.GetExtension(mimeType);
            MemoryStream mStream = new MemoryStream();
            await file.CopyToAsync(mStream);
            File.WriteAllBytes(path, mStream.ToArray());
            return path;
        }

        protected override void Dispose(bool disposing) {

            if (disposing && _userManager != null && _db != null && _imageManager != null) {
                _userManager.Dispose();
                _userManager = null;
                _db.Dispose();
                _db = null;
                _imageManager = null;
            }

            base.Dispose(disposing);
        }
    }
}
