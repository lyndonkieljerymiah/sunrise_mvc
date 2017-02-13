﻿using System;
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
        
        public static MvcHtmlString NgInputFor<TModel,TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel,TValue>> expression, 
            string controller="",
            string type="text", 
            string[] attribs = null)
        {
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);

            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var value = metadata.Model;
            
            fullBindingName = fullBindingName.ToCamelCase();
            fieldId = fieldId.ToCamelCase();

            StringBuilder builder = new StringBuilder();
            builder.Append("<input ");
            builder.Append("name= '" + fullBindingName + "' ");
            builder.Append("id= '" + fullBindingName + "' ");
            builder.Append("ng-model= '" + controller + "." + fullBindingName + "' ");
            builder.Append("type= '" + type + "' ");
            builder.Append("class= 'form-control " + (type.ToLower()=="amount" ? " text-right' " : "' "));
            var validationAttributes = html.GetUnobtrusiveValidationAttributes(fullBindingName, metadata);
            foreach (var key in validationAttributes.Keys)
            {   
                builder.Append(key + "='" + validationAttributes[key].ToString() + "' ");
            }
            string strAttribute = attribs == null ? "" : string.Join(" ", attribs);
            builder.Append(strAttribute);
            builder.Append(" />");

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString NgHiddenFor<TModel,TValue>(this HtmlHelper<TModel> html,Expression<Func<TModel,TValue>> expression,string controller)
        {
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);

            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var value = metadata.Model;

            fullBindingName = fullBindingName.ToCamelCase();
            fieldId = fieldId.ToCamelCase();

            StringBuilder builder = new StringBuilder();
            builder.Append("<input ");
            builder.Append("name= '" + fullBindingName + "' ");
            builder.Append("id= '" + fullBindingName + "' ");
            builder.Append("ng-model= '" + controller + "." + fullBindingName + "' ");
            builder.Append("type= 'hidden' ");
            builder.Append(" />");

            return new MvcHtmlString(builder.ToString());
        }
        public static MvcHtmlString NgDropdownFor<TModel,TValue>(this HtmlHelper<TModel> html,Expression<Func<TModel,TValue>> expression, string optionModel="",string controller="", string[] attribs=null)
        {
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);

            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var value = metadata.Model;


            fullBindingName = fullBindingName.ToCamelCase();
            fieldId = fieldId.ToCamelCase();

            StringBuilder builder = new StringBuilder();
            string strAttribute = attribs == null ? "" : string.Join(" ", attribs);

            builder.Append(@"<select 
                    name='" + fullBindingName +
                    "' id='" + fullBindingName +
                    "' ng-model='" + controller + "." + fullBindingName +
                    "' class='form-control'" + strAttribute + " ");

            var validationAttributes = html.GetUnobtrusiveValidationAttributes(fullBindingName, metadata);
            foreach (var key in validationAttributes.Keys)
            {
                builder.Append(key + "='" + validationAttributes[key].ToString() + "' ");
            }

            builder.Append(">");
            builder.Append("<option ng-repeat='item in " + optionModel + "' value='{{item.value}}'>{{item.text}}</option>");
            builder.Append("</select>");
            var dropDownString = builder.ToString();
            
            return new MvcHtmlString(dropDownString);
        }
        public static MvcHtmlString NgTextAreaFor<TModel,TValue>(this HtmlHelper<TModel> html,Expression<Func<TModel,TValue>> expression, string controller="", string[] attribs = null)
        {

            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);

            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var value = metadata.Model;

            fullBindingName = fullBindingName.ToCamelCase();
            fieldId = fieldId.ToCamelCase();

            StringBuilder builder = new StringBuilder();
            builder.Append("<textarea ");
            builder.Append("name= '" + fullBindingName + "' ");
            builder.Append("id= '" + fullBindingName + "' ");
            builder.Append("ng-model= '" + controller + "." + fullBindingName + "' ");
            builder.Append("class= 'form-control' ");
            
            var validationAttributes = html.GetUnobtrusiveValidationAttributes(fullBindingName, metadata);
            foreach (var key in validationAttributes.Keys)
            {
                builder.Append(key + "='" + validationAttributes[key].ToString() + "' ");
            }

            builder.Append(" ></textarea>");
            return new MvcHtmlString(builder.ToString());
        }
        public static MvcHtmlString NgDatePickerFor<TModel,TValue>(this HtmlHelper<TModel> html,Expression<Func<TModel,TValue>> expression, string controller = "",string index="", string[] attribs = null)
        {
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);

            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var value = metadata.Model;

            fullBindingName = fullBindingName.ToCamelCase();
            fieldId = fieldId.ToCamelCase();

            StringBuilder builder = new StringBuilder();
            builder.Append("<date-time-picker ");
            builder.Append("my-name= '" + fullBindingName + "' ");
            builder.Append("model= '" + controller + "." + fullBindingName + "' ");
            builder.Append("index= '" + index + "' ");

            var validationAttributes = html.GetUnobtrusiveValidationAttributes(fullBindingName, metadata);
            foreach (var key in validationAttributes.Keys)
            {
                builder.Append(key + "=" + validationAttributes[key].ToString() + " ");
            }

            builder.Append(" ></date-time-picker>");
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString NgValidateFor<TModel,TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            string controller = "")
        {
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);

            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var value = metadata.Model;

            fullBindingName = fullBindingName.ToCamelCase();
            fieldId = fieldId.ToCamelCase();

            StringBuilder builder = new StringBuilder();
            builder.Append("<span ng-show=\"errorState.error['" + controller + "." + fullBindingName +"']\" >");
            builder.Append("{{errorState.error['" + controller + "." + fullBindingName + "']}}");
            builder.Append("</span>");

            return new MvcHtmlString(builder.ToString());
        }
    }
}
