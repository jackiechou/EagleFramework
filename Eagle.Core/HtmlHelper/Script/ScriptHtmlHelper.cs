using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Eagle.Core.HtmlHelper.Script
{
    /// <summary>
    /// @{
    //Html.BeginScriptContext();
    //Html.AddScriptBlock(@"$(function() { if (console) { console.log('rendered from the view'); } });");
    //Html.AddScriptFile("~/Scripts/jquery-ui-1.8.11.js");
    //Html.EndScriptContext();
    //}
    //using (Html.BeginScriptContext())
    //  {
    //    Html.AddScriptFile("~/Scripts/jquery.validate.js");
    //    Html.AddScriptFile("~/Scripts/jquery-ui-1.8.11.js");
    //    Html.AddScriptBlock(@"$(function() {
    //        $('#someField').datepicker();
    //    });");
    //  }
  //  using (var context = Html.BeginScriptContext())
  //{
  //  context.ScriptFiles.Add("~/Scripts/jquery-1.5.1.min.js");
  //  context.ScriptFiles.Add("~/Scripts/modernizr-1.7.min.js");
  //} 
  //Html.ScriptFile("~/Scripts/jquery-ui-1.8.11.js");
  //  Html.ScriptBlock(@"$(function() { if (console) { console.log('rendered from the view'); } });");
    /// </summary>
    public static class ScriptHtmlHelper
    {
        public static ScriptContext BeginScriptContext(this System.Web.Mvc.HtmlHelper htmlHelper)
        {
            var scriptContext = new ScriptContext(htmlHelper.ViewContext.HttpContext);
            htmlHelper.ViewContext.HttpContext.Items[ScriptContext.ScriptContextItem] = scriptContext;
            return scriptContext;
        }

        public static void EndScriptContext(this System.Web.Mvc.HtmlHelper htmlHelper)
        {
            var items = htmlHelper.ViewContext.HttpContext.Items;
            var scriptContext = items[ScriptContext.ScriptContextItem] as ScriptContext;

            if (scriptContext != null)
            {
                scriptContext.Dispose();
            }
        }

        public static void AddScriptBlock(this System.Web.Mvc.HtmlHelper htmlHelper, string script)
        {
            var scriptGroup = htmlHelper.ViewContext.HttpContext.Items[ScriptContext.ScriptContextItem] as ScriptContext;

            if (scriptGroup == null)
                throw new InvalidOperationException("cannot add a script block without a script context. Call Html.BeginScriptContext() beforehand.");

            scriptGroup.ScriptBlocks.Add(script);
        }

        public static void AddScriptFile(this System.Web.Mvc.HtmlHelper htmlHelper, string path)
        {
            var scriptGroup = htmlHelper.ViewContext.HttpContext.Items[ScriptContext.ScriptContextItem] as ScriptContext;

            if (scriptGroup == null)
                throw new InvalidOperationException("cannot add a script file without a script context. Call Html.BeginScriptContext() beforehand.");

            scriptGroup.ScriptFiles.Add(path);
        }

        public static IHtmlString RenderScripts(this System.Web.Mvc.HtmlHelper htmlHelper)
        {
            var scriptContexts = htmlHelper.ViewContext.HttpContext.Items[ScriptContext.ScriptContextItems] as Stack<ScriptContext>;

            if (scriptContexts != null)
            {
                var count = scriptContexts.Count;
                var builder = new StringBuilder();
                var script = new List<string>();
                var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection);

                for (int i = 0; i < count; i++)
                {
                    var scriptContext = scriptContexts.Pop();

                    foreach (var scriptFile in scriptContext.ScriptFiles)
                    {
                        builder.AppendLine("<script type='text/javascript' src='" + urlHelper.Content(scriptFile) + "'></script>");
                    }

                    script.AddRange(scriptContext.ScriptBlocks);

                    // render out all the scripts in one block on the last loop iteration
                    if (i == count - 1)
                    {
                        builder.AppendLine("<script type='text/javascript'>");
                        foreach (var s in script)
                        {
                            builder.AppendLine(s);
                        }
                        builder.AppendLine("</script>");
                    }
                }

                return new MvcHtmlString(builder.ToString());
            }

            return MvcHtmlString.Empty;
        }
    }
}
