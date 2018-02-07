using System;

namespace Eagle.Entities
{
    /// <summary>
    /// Clean Entities are those at utilize UTC datetimes.
    /// EF DataContext looks for this when converting DateTime.Kind to UTC.
    /// Once all entities utilize UTC, CleanEntityBase will become the default
    /// </summary>
    [Serializable]
    public abstract class CleanEntityBase : EntityBase{}
}