namespace Eagle.Services.Dtos.SystemManagement
{
    public class MenuTypeDetail : DtoBase
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
