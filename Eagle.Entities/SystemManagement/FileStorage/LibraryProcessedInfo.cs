namespace Eagle.Entities.SystemManagement.FileStorage
{
    public class LibraryProcessedInfo : EntityBase
    {

        public int LibraryProcessedInfoId { get; set; }

        public int ArtifactId { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public int? Pages { get; set; }
        
        public string ArtifactType { get; set; }

        public bool UseOriginal { get; set; }

        public int? FileStoreId { get; set; }

        public virtual DocumentFile FileStore { get; set; }

    }
}
