using System;

namespace CommonShares.Entities
{
    public abstract class Entity<TId> : IEntity<TId>
    {
        public virtual TId Id { get; set; }

        public bool IsTransient()
        {
            return Id.Equals(default(TId));
        }

        #region Override Methods
        public override bool Equals(object entity)
        {
            
            if((entity==null) || !(entity is Entity<TId>))
            {
                return false;
            }

            if(object.ReferenceEquals(this,entity))
            {
                return true;
            }

            Entity<TId> item = (Entity<TId>)entity;

            if(item.IsTransient() || this.IsTransient())
            {
                return false;
            }
            else
            {
                return item.Id.Equals(this.Id);
            }

            
        }

        private int? _requestedHashCode;

           public override int GetHashCode()
        {

            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31;

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }

        public static bool operator ==(Entity<TId> left,
           Entity<TId> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity<TId> left,
            Entity<TId> right)
        {
            return (!(left == right));
        }

        #endregion
    }
}
