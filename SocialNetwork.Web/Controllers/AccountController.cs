using Newtonsoft.Json.Linq;
using SocialNetwork.Web.Helpers;
using SocialNetwork.Web.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Web.Controllers {
    public class AccountController : Controller {

        private HttpClient _client;
        private TokenHelper _tokenHelper;

        public AccountController() {
            _client = new HttpClient();

            _client.BaseAddress = new Uri("https://localhost:44323/");
            _client.DefaultRequestHeaders.Accept.Clear();

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(mediaType);

            _tokenHelper = new TokenHelper();
        }
        // GET: Account/Register
        public ActionResult Register() {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {
                var response = await _client.PostAsJsonAsync("api/Account/Register", model);

                if (response.IsSuccessStatusCode) {
                    HttpContext.Response.RedirectToRoute("Login");
                } else {
                    //ToDo
                }
            }
            return View(model);
        }

        // GET: Account/Login
        public ActionResult Login() {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model) {
            if (ModelState.IsValid) {
                var data = new Dictionary<string, string>() {
                    { "grant_type", "password" },
                    { "username", model.Email },
                    { "password", model.Password }
                };

                using (var requestContent = new FormUrlEncodedContent(data)) {
                    var response = await _client.PostAsync("/Token", requestContent);

                    if (response.IsSuccessStatusCode) {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var tokenData = JObject.Parse(responseContent);

                        _tokenHelper.AccessToken = tokenData["access_token"];
                        _tokenHelper.UserId = tokenData["user_id"];
                        _tokenHelper.UserName = tokenData["user_name"];
                        _tokenHelper.HasProfile = tokenData["has_profile"];

                        return RedirectToAction("Index", "Home");
                    } else {
                        ModelState.AddModelError("", "");
                    }
                }
            }

            return View(model);
        }

        // POST: Account/Logoff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff() {
            _tokenHelper.AccessToken = null;
            _tokenHelper.UserId = null;
            _tokenHelper.UserName = null;
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Confirm
        public async Task<ActionResult> Confirm(string token, string u) {
            if (ModelState.IsValid) {
                var encodedToken = HttpUtility.UrlEncode(token);
                var encodedUserId = HttpUtility.UrlEncode(u);

                var response = await _client.GetAsync(_client.BaseAddress + $"api/Account/Confirm?token={encodedToken}&userId={encodedUserId}");                
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Account/PasswordRecovery
        public ActionResult PasswordRecovery() {
            return View();
        }

        // POST: Account/PasswordRecovery
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PasswordRecovery(string email) {
            var data = new Dictionary<string, string>() {
                    { "Email", email }
            };
            using (var requestContent = new FormUrlEncodedContent(data)) {
                var response = await _client.PostAsync("api/Account/PasswordRecovery", requestContent);
            }
            return View();
        }

        // GET: Account/ChangePassword
        public ActionResult ChangePassword(string token, string u) {
            ViewBag.Token = token;
            ViewBag.UserId = u;
            return View();
        }

        // POST: Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(PasswordRecoveryViewModel model) {
            var data = new Dictionary<string, string>() {
                {"UserId", model.UserId },
                {"Token", model.Token },
                {"NewPassword", model.NewPassword }
            };
            using(var requestContent = new FormUrlEncodedContent(data)) {
                var response = await _client.PostAsync("api/Account/ChangePassword", requestContent);
            }
            return View();
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