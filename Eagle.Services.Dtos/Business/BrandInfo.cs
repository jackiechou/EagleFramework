using System;
using System.Collections.Generic;

namespace Eagle.Services.Dtos.Business
{
    public class BrandInfo : DtoBase
    {
        public int? BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandAlias { get; set; }
        public bool? IsOnline { get; set; }
    }
}
