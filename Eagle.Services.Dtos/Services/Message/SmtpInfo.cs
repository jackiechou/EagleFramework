namespace Eagle.Services.Dtos.Services.Message
{
    public class SmtpInfo : DtoBase
    {
        public string SmtpServer { get; set; }
        public string SmtpAuthentication { get; set; }
        public string SmtpmEmail { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string MailSignature { get; set; }
        public bool EnableSsl { get; set; }
    }
}
