namespace Eagle.Core.HtmlHelper.CheckBox
{
    public class CheckBoxListInfo
    {
        public string Value { get; private set; }
        public string DisplayText { get; private set; }
        public bool IsChecked { get; private set; }
        public CheckBoxListInfo(string value, string displayText, bool isChecked)
        {
            Value = value;
            DisplayText = displayText;
            IsChecked = isChecked;
        }
    }
}
