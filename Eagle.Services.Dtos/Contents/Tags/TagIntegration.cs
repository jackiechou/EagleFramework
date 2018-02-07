using System;
using Eagle.Core.Settings;
using Newtonsoft.Json;

namespace Eagle.Services.Dtos.Contents.Tags
{
    public class TagIntegrationDetail : DtoBase
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int TagIntegrationId { get; set; }
        public TagType TagType { get; set; }
        public int TagKey { get; set; }
        public int TagId { get; set; }
        public TagStatus TagStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public TagDetail TagDetail { get; set; }
    }

    public class TagIntegrationEntry : DtoBase
    {
        public int TagKey { get; set; }
        public int TagId { get; set; }
        public TagType TagType { get; set; }
        public TagStatus TagStatus { get; set; }
    }
}