using System.ComponentModel;

namespace Eagle.Core.Extension
{
    /// <summary>
    /// SignalR EventName enumeration.
    /// Note : The Enum Description should match the defined SignalR method
    /// </summary>
    public enum EventNameEnum
    {
        /// <summary>
        /// Unknown enum
        /// </summary>
        UNKNOWN = 0,

        /// <summary>
        /// On Message Listened(onMessageListened) enum value. For general purpose
        /// </summary>
        [Description("onMessageListened")]
        ON_MESSAGE_LISTENED = 1,

        /// <summary>
        /// On Inserted(onInserted) enum value.
        /// </summary>
        [Description("onInserted")]
        ON_INSERTED = 2,

        /// <summary>
        /// On Deleted(onDeleted) enum value.
        /// </summary>
        [Description("onDeleted")]
        ON_DELETED = 3,

        /// <summary>
        /// On Updated(onUpdated) enum value.
        /// </summary>
        [Description("onUpdated")]
        ON_UPDATED = 4,

        /// <summary>
        /// On Exception(onException) enum value.
        /// </summary>
        [Description("onException")]
        ON_EXCEPTION = 5
    }
}
