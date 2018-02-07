using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement.FileStorage;
using System;
using Eagle.Core.Pagination;

namespace Eagle.Services.Dtos.Contents.Documentation
{
    public class DocumentationDetail : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "DocumentationId")]
        public int DocumentationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileId")]
        public int FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public DocumentationStatus Status { get; set; }
    }

    public class DocumentationInfoDetail : DocumentationDetail
    {
        public DocumentInfoDetail DocumentInfo { get; set; }
    }
    public class DocumentationLink : BaseDto
    {
        public DocumentInfoDetail FileInfo { get; set; }
        public string ViewLink { get; set; }
        public string DownloadLink { get; set; }
    }


    public class DocumentationEntry : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "File")]
        public HttpPostedFileBase File { get; set; }
    }

    public class DocumentationEditEntry : DocumentationEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "DocumentationId")]
        public int DocumentationId { get; set; }

        public DocumentInfoDetail DocumentInfo { get; set; }
    }

    public class DocumentationSearchEntry : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Keywords")]
        [StringLength(400, MinimumLength = 2, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MinMaxTitleLength")]
        public string Keywords { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public DocumentationStatus? Status { get; set; }
    }

    public class DocumentationSearchResult : BaseDto
    {
        public DocumentationSearchResult(DocumentationSearchEntry filter, IPagedList<DocumentationInfoDetail> pagedList)
        {
            Filter = filter;
            PagedList = pagedList;
        }
        public DocumentationSearchEntry Filter { get; set; }
        public IPagedList<DocumentationInfoDetail> PagedList { get; set; }
    }
}
