using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business.Personnel
{
    public class QualificationDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "QualificationId")]
        public int QualificationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "QualificationNo")]
        public string QualificationNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "QualificationDate")]
        public DateTime? QualificationDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileId")]
        public int? FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Note")]
        public string Note { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        public int EmployeeId { get; set; }
    }
    public class QualificationEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "QualificationNo")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string QualificationNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "QualificationDate")]
        public DateTime? QualificationDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileId")]
        public int? FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Note")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Note { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "File")]
        public HttpPostedFileBase File { get; set; }
    }
    public class QualificationEditEntry : QualificationEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "QualificationId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int QualificationId { get; set; }
    }
}
