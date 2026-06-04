using System;
using System.Text;
using System.Web.Mvc;
using FirstAppRadiuk.Models;

namespace FirstAppRadiuk.Helpers
{
    public static class PagingHelpers
    {
        // Хелпер генерує кнопки-посилання для переходу між сторінками
        // pageInfo - інформація про пагінацію
        // pageUrl - функція що будує URL для кожної сторінки
        public static MvcHtmlString PageLinks(this HtmlHelper html,
            PageInfo pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pageInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                // Поточна сторінка виділяється синім кольором
                if (i == pageInfo.PageNumber)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}