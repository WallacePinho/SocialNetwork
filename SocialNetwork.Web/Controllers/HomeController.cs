using SocialNetwork.Web.Attributes;
using SocialNetwork.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Web.Controllers {
    public class HomeController : Controller {

        private HttpClient _client;
        private TokenHelper _tokenHelper;

        public HomeController() {
            _client = new HttpClient();

            _client.BaseAddress = new Uri("https://localhost:44323/");
            _client.DefaultRequestHeaders.Accept.Clear();

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(mediaType);

            _tokenHelper = new TokenHelper();
        }


        // GET: Home
        [Authentication]
        public ActionResult Index() {
            if (_tokenHelper.HasProfile != null && _tokenHelper.HasProfile.ToString().ToLower() == "false") {
                return RedirectToAction("Create", "Profile");
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