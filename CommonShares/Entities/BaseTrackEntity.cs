using System;

namespace CommonShares.Entities
{
    public abstract class BaseTrackEntity<TId>: Entity<TId>
    {
        public DateTimeOffset CreateDateTimeOffset { get; set; }= DateTimeOffset.UtcNow;
        public string CreatedByUserID { get; set; }
        public DateTimeOffset UpdateTimeOffset { get; set; }= DateTimeOffset.UtcNow;
        public string UpdateByUserID { get; set; }
    }
}
