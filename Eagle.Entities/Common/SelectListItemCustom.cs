using System.Collections.Generic;
using System.Web.Mvc;

namespace Eagle.Entities.Common
{
    public class SelectListItemCustom : SelectListItem
    {
        public IDictionary<string, object> ItemsHtmlAttributes { get; set; }
    }
}
