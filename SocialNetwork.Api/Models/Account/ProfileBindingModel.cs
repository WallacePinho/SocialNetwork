using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SocialNetwork.Api.Models.Account {
    public class ProfileBindingModel {
        public PostedFile ProfilePicture { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string UserName { get; set; }
    }
    public class PostedFile {
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public byte[] InputStream { get; set; }
    }
}