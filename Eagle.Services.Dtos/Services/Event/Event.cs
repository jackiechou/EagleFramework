using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement.FileStorage;

namespace Eagle.Services.Dtos.Services.Event
{
    public class EventSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Keywords")]
        [StringLength(400, MinimumLength = 2, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MinMaxTitleLength")]
        public string Keywords { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FromDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ToDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public EventStatus? SearchStatus { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int? TypeId { get; set; }
    }
    public class EventInfoDetail : EventDetail
    {
        public EventTypeDetail EventType { get; set; }
        public List<DocumentInfoDetail> DocumentInfos { get; set; }
    }
    public class EventDetail : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "EventId")]
        public int EventId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EventCode")]
        public string EventCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EventTitle")]
        public string EventTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EventMessage")]
        public string EventMessage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TimeZone")]
        public string TimeZone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsNotificationUsed")]
        public bool IsNotificationUsed { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EventStatus")]
        public EventStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Location")]
        public string Location { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Latitude")]
        public double? Latitude { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Longitude")]
        public double? Longitude { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SmallPhoto")]
        public int? SmallPhoto { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LargePhoto")]
        public int? LargePhoto { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }
    }
    public class EventEntry : DtoBase
    {
        public EventEntry()
        {
            StartDate = DateTime.UtcNow;
            Status = EventStatus.Upcoming;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectType")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EventCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(32, MinimumLength = 3, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength")]
        public string EventCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(256, MinimumLength = 3, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength")]
        public string EventTitle { get; set; }


        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string EventMessage { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TimeZone")]
        [StringLength(256, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string TimeZone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Location")]
        public string Location { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsNotificationUsed")]
        public bool IsNotificationUsed { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public EventStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "File")]
        public HttpPostedFileBase File { get; set; }
    }
    public class EventEditEntry : EventEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "EventId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int EventId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SmallPhoto")]
        public int? SmallPhoto { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LargePhoto")]
        public int? LargePhoto { get; set; }

        //public EventTypeDetail EventType { get; set; }
        public List<DocumentInfoDetail> DocumentInfos { get; set; }
    }
}
