using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EKContent.web.Infrastructure
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString Help(this HtmlHelper helper, string text)
        {
            return new MvcHtmlString(String.Format("<p><em><small>({0})</small></em></p>", text));
        }
    }
}