namespace Eagle.Core.HtmlHelper.CheckBox.Internal
{
    public static partial class Extensions
    {
        private static ListBuilder _listBuilder;
        static Extensions()
        {
            _listBuilder = new ListBuilder();
        }
    }
}
