using Eagle.Services.Service.MessageBroadCaster;

namespace Eagle.Services.Service.HubConfiguration
{
    /// <summary>
    /// DataBase Hub Listener interface
    /// </summary>
    public interface IDbListener
    {
        #region Public Properties 

        /// <summary>
        /// Get Insert Listener
        /// </summary>
        IBroadCastListener InsertListener { get; }

        /// <summary>
        /// Get Update Listener
        /// </summary>
        IBroadCastListener UpdateListener { get; }

        /// <summary>
        /// Get Delete Listener
        /// </summary>
        IBroadCastListener DeleteListener { get; }

        #endregion
    }
}
