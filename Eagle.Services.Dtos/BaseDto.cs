using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos
{
    public class BaseDto: DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Ip")]
        public string Ip { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastUpdatedIp")]
        public string LastUpdatedIp { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        public DateTime? LastModifiedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedByUserId")]
        public Guid? CreatedByUserId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedByUserId")]
        public Guid? LastModifiedByUserId { get; set; }
    }
}
