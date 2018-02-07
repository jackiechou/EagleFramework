namespace Eagle.Services.Dtos.SystemManagement
{
    public class CountryDetail : DtoBase
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string Iso { get; set; }
        public string Iso3 { get; set; }
        public int NumCode { get; set; }
        public int? PhoneCode { get; set; }
        public bool IsActive { get; set; }
    }

    public class CountryEntry : DtoBase
    {
        public string CountryName { get; set; }
        public string NiceName { get; set; }
        public string Iso { get; set; }
        public string Iso3 { get; set; }
        public int? NumCode { get; set; }
        public int PhoneCode { get; set; }
    }
}
