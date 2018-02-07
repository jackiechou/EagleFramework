namespace Eagle.Services.Dtos.Common
{
    public class ExceptionInfo: DtoBase
    {
        public int StatusCode { get; set; }
        public string ImageUrl { get; set; }
        public string PageTitle { get; set; }
        public string Title { get; set; }
        public string ErrorMessage { get; set; }

        public ExceptionInfo(int statusCode, string pageTitle=null, string title=null, string errorMessage = null, string imageUrl = null)
        {
            StatusCode = statusCode;
            PageTitle = pageTitle;
            Title = title;
            ErrorMessage = errorMessage;
            ImageUrl = imageUrl;
        }
    }
}
