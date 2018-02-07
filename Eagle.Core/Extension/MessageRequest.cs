namespace Eagle.Core.Extension
{
    /// <summary>
    /// Message Request class
    /// </summary>
    public class MessageRequest
    {
        /// <summary>
        /// Get or set Message value
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Get or set EventName
        /// </summary>
        public EventNameEnum EventName { get; set; }
    }
}
