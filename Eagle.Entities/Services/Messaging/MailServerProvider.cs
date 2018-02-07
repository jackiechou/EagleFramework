using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Messaging
{
    [Table("MailServerProvider",Schema = "Messaging")]
    public class MailServerProvider : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MailServerProviderId { get; set; }
        public string MailServerProviderName { get; set; }
        public string MailServerProtocol { get; set; }
        public string IncomingMailServerHost { get; set; }
        public int? IncomingMailServerPort { get; set; }
        public string OutgoingMailServerHost { get; set; }
        public int? OutgoingMailServerPort { get; set; }
        public bool Ssl { get; set; }
        public bool Tls { get; set; }
    }
}
