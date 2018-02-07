namespace Eagle.Services.Dtos.SystemManagement
{
    public class RegionDetail : DtoBase
    {
        public int RegionId { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public bool IsActive { get; set; }
        public int ProvinceId { get; set; }
    }

    public class RegionEntry : DtoBase
    {
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public bool IsActive { get; set; }
        public int ProvinceId { get; set; }
    }
}
