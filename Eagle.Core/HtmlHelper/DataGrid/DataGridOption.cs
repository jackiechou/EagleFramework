namespace Eagle.Core.HtmlHelper.DataGrid
{
    public class DataGridOption
    {
        public DataGridOption()
        {
            EditAction = "Edit";
            EditButtonText = "Edit";
            ShowEditButton = true;

            DeleteAction = "Edit";
            DeleteButtonText = "Delete";
            ShowDeleteButton = true;
        }

        public string[] Columns { get; set; }

        public string EditAction { get; set; }
        public string EditButtonText { get; set; }
        public bool ShowEditButton { get; set; }

        public string DeleteAction { get; set; }
        public string DeleteButtonText { get; set; }
        public bool ShowDeleteButton { get; set; } 
    }
}
