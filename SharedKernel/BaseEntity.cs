using FPTS.FIT.BDRD.BuildingBlocks.SharedKernel.Interfaces;

namespace FPTS.FIT.BDRD.BuildingBlocks.SharedKernel
#nullable disable
{
    public abstract class BaseEntity
    {
        int _Id;
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
        private List<BaseDomainEvent> _domainEvents;
        public IReadOnlyCollection<BaseDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(BaseDomainEvent eventItem)
        {
            _domainEvents = _domainEvents ?? new List<BaseDomainEvent>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(BaseDomainEvent eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
