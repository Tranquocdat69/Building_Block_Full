using MediatR;

namespace FPTS.FIT.BDRD.BuildingBlocks.SharedKernel
{
    public abstract class BaseDomainEvent : INotification
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.Now;
    }
}