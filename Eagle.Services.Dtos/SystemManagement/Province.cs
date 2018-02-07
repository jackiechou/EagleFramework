namespace Eagle.Services.Dtos.SystemManagement
{
    public class ProvinceDetail : DtoBase
    {
        public int ProvinceId { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public int ListOrder { get; set; }
        public bool IsActive { get; set; }

        public int CountryId { get; set; }
    }

    public class ProvinceEntry : DtoBase
    {
        public int CountryId { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public int ListOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
