using Eagle.Common.Settings;
using Microsoft.Azure;

namespace Eagle.Core.Configuration
{
    public class ConfigurationProvider: IConfigurationProvider
    {
        public string FileTransferApiKey => ConfigSettings.ReadSetting("RackspaceApikey").Trim();
        public string FileTransferUserName => ConfigSettings.ReadSetting("RackspaceUsername").Trim();
        public string FileTransferPassword => ConfigSettings.ReadSetting("RackspacePassword").Trim();
        public string RackspaceContainerTimeToLive => ConfigSettings.ReadSetting("RackspaceContainerTimeToLive").Trim();
        public string MemberAvatarDirectory => ConfigSettings.ReadSetting("MemberAvatarDirectory").Trim();
        public string LibraryDocumentLocalUrl => ConfigSettings.ReadSetting("LibraryDocumentLocalUrl").Trim();
        public string BlobStorageConnectionString => CloudConfigurationManager.GetSetting("TableStorageConnectionString").Trim();
    }
}