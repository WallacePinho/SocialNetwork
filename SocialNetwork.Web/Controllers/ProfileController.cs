using Newtonsoft.Json;
using SocialNetwork.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Web.Models.Account;
using Newtonsoft.Json.Linq;
using System.Text;
using MimeTypes;
using System.IO;
using SocialNetwork.Web.Attributes;

namespace SocialNetwork.Web.Controllers {
    public class ProfileController : Controller {

        private HttpClient _client;
        private TokenHelper _tokenHelper;

        public ProfileController() {
            _client = new HttpClient();

            _client.BaseAddress = new Uri("https://localhost:44323/");
            _client.DefaultRequestHeaders.Accept.Clear();

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(mediaType);

            _tokenHelper = new TokenHelper();
        }

        // GET: Profile
        public async Task<ActionResult> Index(string userName) {

            var response = await _client.GetAsync(_client.BaseAddress + $"api/Profile?userName={userName}");

            if (response.IsSuccessStatusCode) {
                var responseContent = await response.Content.ReadAsStringAsync();

                var profile = JsonConvert.DeserializeObject<ProfileViewModel>(responseContent);

                return View(profile);
            } else {
                return View();
            }
        }

        [Authentication]
        public ActionResult Create() {
            if (_tokenHelper.HasProfile.ToString().ToLower() == "true") {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [Authentication]
        [HttpPost]
        public async Task<ActionResult> Create(ProfileBindingModel model) {
            var json = JsonConvert.SerializeObject(model, new HttpPostedFileConverter());

            using (var requestContent = new StringContent(json, Encoding.UTF8, "application/json")) {
                var userId = _tokenHelper.UserId;
                var response = await _client.PostAsync($"api/Profile?userId={userId}", requestContent);
                
                if (response.IsSuccessStatusCode) {
                    _tokenHelper.HasProfile = true;
                    return RedirectToAction("Index", new { userName = model.UserName });
                } else {
                    return View();
                }
            }
        }

        protected override void Dispose(bool disposing) {
            if (disposing && _client != null) {
                _client.Dispose();
                _client = null;
            }
            base.Dispose(disposing);
        }
    }

    
}