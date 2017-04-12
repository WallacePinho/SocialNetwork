using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Web.Helpers {
    public static class HtmlHelpers {
        public static MvcHtmlString Image(this HtmlHelper helper, string src, string altText, string height) {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", src);
            builder.MergeAttribute("alt", altText);
            builder.MergeAttribute("height", height);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString InputFile(this HtmlHelper helper, string name, string htmlClass) {
            var builder = new TagBuilder("input");
            builder.MergeAttribute("id", name);
            builder.MergeAttribute("name", name);
            builder.MergeAttribute("type", "file");
            builder.MergeAttribute("class", htmlClass);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}