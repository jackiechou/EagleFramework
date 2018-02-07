using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class BrandDetail : DtoBase
    {
        public int BrandId { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "BrandName")]
        public string BrandName { get; set; }
        public string BrandAlias { get; set; }
        public bool IsOnline { get; set; }
        public int? FileId { get; set; }
    }
}
