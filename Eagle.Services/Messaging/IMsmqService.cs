using Eagle.Services.Dtos.Services.Message;

namespace Eagle.Services.Messaging
{
    public interface IMsmqService : IBaseService
    {
        string QueueMessage(EmailMessage mailMessage);
    }
}
