using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Eagle.Core.Pagination
{
    public static class PagingHtmlHelper
    {
        #region HtmlHelper extensions
        //<div class=""pager"">
        //   @Html.Pager(Model.PageSize, Model.PageNumber, Model.TotalItemCount)
        //</div>
        //public ActionResult Index(int? page)
        //{
        //    int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
        //    return View(this.allProducts.ToPagedList(currentPageIndex, defaultPageSize));
        //}";
        public static Pager Pager(this System.Web.Mvc.HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount)
        {
            return new Pager(htmlHelper, pageSize, currentPage, totalItemCount);
        }
        //<div class="pager">
        //   @Html.Pager(Model.PageSize, Model.PageNumber, Model.TotalItemCount).Options(o => o
        //        .PageRouteValueKey("Search.page")
        //       .AlwaysAddFirstPageNumber())
        //</div>
        public static Pager Pager(this System.Web.Mvc.HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, AjaxOptions ajaxOptions)
        {
            return new Pager(htmlHelper, pageSize, currentPage, totalItemCount).Options(o => o.AjaxOptions(ajaxOptions));
        }

        public static Pager<TModel> Pager<TModel>(this HtmlHelper<TModel> htmlHelper, int pageSize, int currentPage, int totalItemCount)
        {
            return new Pager<TModel>(htmlHelper, pageSize, currentPage, totalItemCount);
        }

        public static Pager<TModel> Pager<TModel>(this HtmlHelper<TModel> htmlHelper, int pageSize, int currentPage, int totalItemCount, AjaxOptions ajaxOptions)
        {
            return new Pager<TModel>(htmlHelper, pageSize, currentPage, totalItemCount).Options(o => o.AjaxOptions(ajaxOptions));
        }

        #endregion
    }
}
