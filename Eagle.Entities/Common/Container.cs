using System;
using Eagle.Entities.SystemManagement;

namespace Eagle.Entities.Common
{
    public class Container : EntityBase
    {
        public int ContainerId { get; set; }
        public string Name { get; set; }

        public Guid? UserId { get; set; }
        public virtual User User { get; set; }

        public ContainerType ContainerType { get; set; }
        public string Visibility { get; set; }
        public string Section { get; set; }
        public string Path { get; set; }
        public string PathDisplay { get; set; }
        public string Description { get; set; }
        public bool? Default { get; set; }
        public TrashStatus TrashStatus { get; set; }
    }

    public class ContainerEntitySummarry : EntityBase
    {
        public int ContainerId { get; set; }
        public string Name { get; set; }
        public ContainerType ContainerType { get; set; }
        public string Path { get; set; }
        public string PathDisplay { get; set; }
        public Guid UserId { get; set; }
        public User Owner { get; set; }
        public int ArtifactCount { get; set; }
        public int ContainerCount { get; set; }
        public int LastModified { get; set; }
    }
}