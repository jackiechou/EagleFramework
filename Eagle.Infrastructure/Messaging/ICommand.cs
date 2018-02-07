namespace Eagle.Infrastructure.Messaging
{
    public interface ICommand
    {
        int Version { get; set; }
    }
}
