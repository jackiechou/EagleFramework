namespace Eagle.Core.Configuration
{
    public interface IConfigurationProvider
    {
        string FileTransferApiKey { get; }
        string FileTransferUserName { get; }
        string FileTransferPassword { get; }
        string MemberAvatarDirectory { get; }
        string LibraryDocumentLocalUrl { get; }
        string RackspaceContainerTimeToLive { get; }
        string BlobStorageConnectionString { get; }
    }
}
