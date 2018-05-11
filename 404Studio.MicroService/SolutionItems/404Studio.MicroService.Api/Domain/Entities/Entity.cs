using System;
using System.Collections.Generic;
using MediatR;

namespace YH.Etms.Settlement.Api.Domain.Entities
{
    public abstract class Entity
    {
        int? _requestedHashCode;
        int _Id;
        bool _isDelete = false;
        public virtual int Id
        {
            get
            {
                return _Id;
            }
            protected set
            {
                _Id = value;
            }
        }

        public virtual bool IsDelete
        {
            get
            {
                return _isDelete;
            }
            protected set
            {
                _isDelete = value;
            }
        }

        public virtual DateTime CreationTime { get; set; } = DateTime.Now;

        private List<INotification> _domainEvents;
        public List<INotification> DomainEvents => _domainEvents;

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public bool TryAddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            if (_domainEvents.Contains(eventItem)) return false;
            AddDomainEvent(eventItem);
            return true;
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            if (_domainEvents is null) return;
            _domainEvents.Remove(eventItem);
        }

        public bool IsTransient()
        {
            return this.Id == default(Int32);
        }

        public void DeleteSelf()
        {
            this.IsDelete = true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity item = (Entity)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id == this.Id;
        }

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
        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        public void SetId(int id) { _Id = id; }
    }
}
