namespace Eagle.Services.Dtos.Common
{
    public class GetResult<T> : DtoBase
    {
        public T Record { get; set; }

        public string Error { get; set; }
    }
}
