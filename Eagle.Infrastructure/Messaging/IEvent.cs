namespace Eagle.Infrastructure.Messaging
{
    public interface IEvent
    {
        int Version { get; set; }
    }
}
