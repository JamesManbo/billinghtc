using GenericRepository.Models;
using Global.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DebtManagement.Domain.Seed
{
    public class Entity : IEntity<int>
    {
        public int Id { get; set; }
        [StringLength(68)]
        public string IdentityGuid { get; set; }
        public string Culture { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int DisplayOrder { get; set; }
        public string OrganizationPath { get; set; }

        int? _requestedHashCode;

        private List<INotification> _domainEvents;

        public Entity()
        {
        }

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            if (_domainEvents == null)
            {
                _domainEvents = new List<INotification> { eventItem };
            }
            else
            {
                _domainEvents.Add(eventItem);
            }
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public bool IsTransient()
        {
            return this.Id == 0;
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
                return item.IdentityGuid == this.IdentityGuid;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31; 
                // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

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

        protected IActionResponse Ok()
        {
            return ActionResponse.Success;
        }

        protected IActionResponse Failed(string message = "Thất bại")
        {
            return ActionResponse.Failed(message);
        }

        protected IActionResponse Failed(ErrorGeneric error)
        {
            return ActionResponse.Failed(error);
        }

        public void Deactivate()
        {
            this.IsActive = false;
        }
    }
}
