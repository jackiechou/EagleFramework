using System.IO;
using System.Web.Mvc;

namespace Eagle.Repositories.Themes
{
    public interface IView
    {
        void Render(ViewContext viewContext, TextWriter writer);
    }
}
