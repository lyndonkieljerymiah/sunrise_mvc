using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sunrise.Client.Infrastructure.Extension
{
    public static class AngularBootstrap
    {

        public static MvcHtmlString NgInput(this HtmlHelper helper, string model,string type="text", string[] cssAttribute=null)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<div class='form-group'>");
            builder.Append("<div class='" + cssAttribute[0] + "'>");
            builder.Append("<input type='"+ type + "' ng-model='" + model +"' class='form-control'/>");
            builder.Append("</div>");
            builder.Append("<div class='form-group'>");

            return MvcHtmlString.Create(builder.ToString());
        }
        public static MvcHtmlString NgInputFor<TModel,TValue>(this HtmlHelper<TModel> html,Expression<Func<TModel,TValue>> expression)
        {
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);

            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var value = metadata.Model;


            fullBindingName = fullBindingName.ToCamelCase();
            fieldId = fieldId.ToCamelCase();

            TagBuilder tag = new TagBuilder("input");
            tag.Attributes.Add("name", fullBindingName);
            tag.Attributes.Add("id", fieldId);
            tag.Attributes.Add("ng-model", fullBindingName);
            tag.Attributes.Add("type", "text");
            tag.Attributes.Add("value", value == null ? "" : value.ToString());
            tag.AddCssClass("form-control");

            var validationAttributes = html.GetUnobtrusiveValidationAttributes(fullBindingName, metadata);
            foreach (var key in validationAttributes.Keys)
            {
                tag.Attributes.Add(key, validationAttributes[key].ToString());
            }

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }
        public static MvcHtmlString NgDropdownFor<TModel,TValue>(this HtmlHelper<TModel> html,Expression<Func<TModel,TValue>> expression, string options, object actions=null)
        {
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);

            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var value = metadata.Model;


            fullBindingName = fullBindingName.ToCamelCase();
            fieldId = fieldId.ToCamelCase();
            TagBuilder tag = new TagBuilder("select");
            tag.InnerHtml = "<option ng-repeat='item in " + options +"' value='item.value'>{{item.text}}</option>";
            tag.Attributes.Add("name", fullBindingName);
            tag.Attributes.Add("id", fieldId);
            tag.Attributes.Add("ng-model", fullBindingName);
            tag.AddCssClass("form-control");

            var validationAttributes = html.GetUnobtrusiveValidationAttributes(fullBindingName, metadata);
            foreach (var key in validationAttributes.Keys)
            {
                tag.Attributes.Add(key, validationAttributes[key].ToString());
            }

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

    }
}
