namespace Eagle.Services.Dtos.Common
{
    public class SuccessStatus : ResultStatus
    {
        public SuccessStatus()
        {
            Code = 0;
            Description = "success";
        }
    }
}
