namespace Eagle.Entities.Services.Messaging
{
    public class NotificationSmsMessage:EntityBase
    {
        public int GroupId { get; set; }
        public string UserId { get; set; }
        public string PhoneNo { get; set; }
        public string Message { get; set; }
    }
}
