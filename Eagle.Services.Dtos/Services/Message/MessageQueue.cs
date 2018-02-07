using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Message
{
    public enum MessageQueueResponseStatus
    {
        Failure = 0,
        Success = 1,
    }
    public class MessageQueueResponse : DtoBase
    {
        public int Id { get; set; }
        public MessageQueueResponseStatus? ResponseStatus { get; set; }
        public string ResponseMessage { get; set; }
    }
    public class MessageQueueDetail : DtoBase
    {
        public int QueueId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "From")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Bcc { get; set; }
        public string Cc { get; set; }
        public string Body { get; set; }
        public DateTime? PredefinedDate { get; set; }
        public bool Status { get; set; }
        public int? ResponseStatus { get; set; }
        public string ResponseMessage { get; set; }
    }

    [Serializable]
    public class MessageQueueEntry : DtoBase
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Bcc { get; set; }
        public string Cc { get; set; }
        public DateTime? PredefinedDate { get; set; }
    }

    public class MessageQueueEditEntry : MessageQueueEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "QueueId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidTemplateId")]
        public int QueueId { get; set; }

        public bool Status { get; set; }

        public int? ResponseStatus { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime? SentDate { get; set; }
    }
}
