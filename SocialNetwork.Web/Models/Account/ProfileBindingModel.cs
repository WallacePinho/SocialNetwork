using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SocialNetwork.Web.Models.Account {
    public class ProfileBindingModel {
        public HttpPostedFileBase ProfilePicture { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string UserName { get; set; }
    }
}