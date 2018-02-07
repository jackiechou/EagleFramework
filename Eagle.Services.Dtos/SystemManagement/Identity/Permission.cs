namespace Eagle.Services.Dtos.SystemManagement.Identity
{
    public class PermissionDetail : DtoBase
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public int DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
    }

    public class PermissionEntry : DtoBase
    {
        public string PermissionName { get; set; }
        public bool IsActive { get; set; }
    }
}
