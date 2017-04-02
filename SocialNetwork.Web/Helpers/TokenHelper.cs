using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.Web.Helpers {
    public class TokenHelper {

        private static HttpContextBase Current {
            get { return new HttpContextWrapper(HttpContext.Current); }
        }

        public object AccessToken {
            get { return Current.Session != null ? Current.Session["AccessToken"] : null; }
            set { if (Current.Session != null) Current.Session["AccessToken"] = value; }
        }

        public object UserId {
            get { return Current.Session != null ? Current.Session["UserId"] : null; }
            set { if (Current.Session != null) Current.Session["UserId"] = value; }
        }

        public object UserName {
            get { return Current.Session != null ? Current.Session["UserName"] : null; }
            set { if (Current.Session != null) Current.Session["UserName"] = value; }
        }
    }
}