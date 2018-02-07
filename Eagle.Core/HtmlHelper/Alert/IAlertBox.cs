namespace Eagle.Core.HtmlHelper.Alert
{
    public interface IAlertBox : IAlertBoxFluentOptions
    {
        IAlertBoxFluentOptions Success();
        IAlertBoxFluentOptions Error();
        IAlertBoxFluentOptions Warning();
        IAlertBoxFluentOptions Info();
    }
}