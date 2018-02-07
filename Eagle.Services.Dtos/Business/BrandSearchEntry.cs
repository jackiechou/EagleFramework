using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Settings;

namespace Eagle.Services.Dtos.Business
{
    public class BrandSearchEntry
    {
        public string SearchText { get; set; }
        public BrandStatus? IsOnline { get; set; }
    }
}
