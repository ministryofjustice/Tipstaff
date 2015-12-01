using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;


namespace Tipstaff
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = startOfWeek - dt.DayOfWeek; return dt.AddDays(diff).Date;
        }
        public static DateTime StartOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }
        public static DateTime StartOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }
    }
}

namespace Tipstaff.Helpers
{

    public static class LabelExtensions
    {
        /// <summary>
        /// Adds red star to end of field name to indicate mandatory field if TestForRequired is set to true
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="TestForRequired"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, bool TestForRequired)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            string resolvedLabelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(resolvedLabelText))
            {
                return MvcHtmlString.Empty;
            }

            if (TestForRequired && metadata.IsRequired)
            {
                var spanTag = new TagBuilder("span");
                spanTag.Attributes.Add("alt", "Mandatory field");
                spanTag.AddCssClass("required-star");
                spanTag.InnerHtml = "*";

                resolvedLabelText = string.Concat(resolvedLabelText, spanTag.ToString(TagRenderMode.Normal));
            }

            var tag = new TagBuilder("label");
            tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tag.InnerHtml = resolvedLabelText;
            tag.AddCssClass("text_label");

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
    }
}