namespace Eagle.Services.Dtos.Common
{
    public abstract class ResultStatus
    {
        public ResultStatusType Code { get; set; }
        public string Description { get; set; }
    }
}
