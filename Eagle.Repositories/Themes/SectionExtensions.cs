using System;
using System.Web.WebPages;

namespace Eagle.Repositories.Themes
{
   /// <summary>
    ///  @this.RedefineSection("TitleSection",@<h1>Default SubLayout title</h1>)
   /// </summary>

    public static class SectionExtensions
    {
        private static readonly object Obj = new object();
        public static HelperResult RenderSection(this WebPageBase page,
                                string sectionName,
                                Func<object, HelperResult> defaultContent)
        {
            if (page.IsSectionDefined(sectionName))
            {
                return page.RenderSection(sectionName);
            }
            else
            {
                return defaultContent(Obj);
            }
        }

        public static HelperResult RedefineSection(this WebPageBase page,
                                string sectionName)
        {
            return RedefineSection(page, sectionName, defaultContent: null);
        }

        public static HelperResult RedefineSection(this WebPageBase page,
                                string sectionName,
                                Func<object, HelperResult> defaultContent)
        {
            if (page.IsSectionDefined(sectionName))
            {
                page.DefineSection(sectionName,
                                   () => page.Write(page.RenderSection(sectionName)));
            }
            else if (defaultContent != null)
            {
                page.DefineSection(sectionName,
                                   () => page.Write(defaultContent(Obj)));
            }
            return new HelperResult(_ => { });
        }
    }
}
