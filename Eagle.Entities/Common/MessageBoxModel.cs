namespace Eagle.Entities.Common
{
    public class MessageBoxModel
    {
        public bool DisplayErrorMessage { get; set; } = true;
        public string PopupTitle { get; set; }
        public string CssClass { get; set; }
        public string Message { get; set; }
    }
}
