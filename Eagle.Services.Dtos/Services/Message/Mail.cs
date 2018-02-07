using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Message
{
    [Serializable]
    public class EmailMessage
    {
        private string _to;

        public string To
        {
            get { return _to; }
            set { _to = value; }
        }
        private string _from;

        public string From
        {
            get { return _from; }
            set { _from = value; }
        }
        private string _subject;

        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
        private string _body;

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }
    }

    public class MailServerProviderDetail : DtoBase
    {
        [Key]
        [Display(ResourceType = typeof(LanguageResource), Name = "MailServerProviderId")]
        public int MailServerProviderId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MailServerProviderName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string MailServerProviderName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MailServerProtocol")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string MailServerProtocol { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IncomingMailServerHost")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string IncomingMailServerHost { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OutgoingMailServerHost")]
        public int IncomingMailServerPort { get; set; }
        public string OutgoingMailServerHost { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OutgoingMailServerPort")]
        public int? OutgoingMailServerPort { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SSL")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool Ssl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TLS")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool Tls { get; set; }
    }
    public class MailServerProviderEntry : DtoBase
    {
        public MailServerProviderEntry()
        {
            IncomingMailServerPort = 25;
            OutgoingMailServerPort = 25;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "MailServerProviderName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string MailServerProviderName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MailServerProtocol")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(10, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string MailServerProtocol { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IncomingMailServerHost")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string IncomingMailServerHost { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IncomingMailServerPort")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int IncomingMailServerPort { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OutgoingMailServerHost")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string OutgoingMailServerHost { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OutgoingMailServerPort")]
        public int? OutgoingMailServerPort { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SSL")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool Ssl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TLS")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool Tls { get; set; }
    }
    public class MailServerProviderEditEntry : MailServerProviderEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MailServerProviderId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int MailServerProviderId { get; set; }
    }
}
