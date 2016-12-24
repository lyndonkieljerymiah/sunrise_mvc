using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sunrise.Client.Helpers.Extension
{
    public static class AngularBootstrap
    {
        public static MvcHtmlString NgInput(this HtmlHelper helper, string name, string value, string[] cssAttribute)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<div class='form-group'>");
            builder.Append("<div class='" + cssAttribute[0] + "'>");

            builder.Append("</div>");
            builder.Append("<div class='form-group'>");

            return MvcHtmlString.Create(builder.ToString());
        }


    }
}
