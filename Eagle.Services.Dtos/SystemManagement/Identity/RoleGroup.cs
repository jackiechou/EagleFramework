using System;

namespace Eagle.Services.Dtos.SystemManagement.Identity
{
    public class RoleGroupDetail : DtoBase
    {
        public int RoleGroupId { get; set; }
        public Guid RoleId { get; set; }
        public Guid GroupId { get; set; }
        public bool? IsDefaultGroup { get; set; }

        public RoleDetail Role { get; set; }
        public GroupDetail Group { get; set; }
    }

    public class RoleGroupEdit : DtoBase
    {
        public int RoleGroupId { get; set; }
        public Guid RoleId { get; set; }
        public Guid GroupId { get; set; }
        public bool? IsDefaultGroup { get; set; }
        public bool? IsAllowed { get; set; }

        public RoleDetail Role { get; set; }
        public GroupDetail Group { get; set; }
    }

    public class RoleGroupInfoDetail : DtoBase
    {
        public int RoleGroupId { get; set; }
        public Guid RoleId { get; set; }
        public Guid GroupId { get; set; }
        public bool? IsDefaultGroup { get; set; }

        public RoleDetail Role { get; set; }
        public GroupDetail Group { get; set; }
    }
}
