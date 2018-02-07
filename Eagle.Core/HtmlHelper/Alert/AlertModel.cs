using System;

namespace Eagle.Core.HtmlHelper.Alert
{
    [Serializable]
    public class AlertModel
    {
        public AlertModel() { }
        public AlertModel(AlertType alertType, string description = null)
        {
            AlertType = alertType;
            Description = description;
        }
        public AlertModel(AlertType alertType, string title, string description = null)
        {
            AlertType = alertType;
            Title = title;
            Description = description;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public AlertType AlertType { get; set; }
    }
}
