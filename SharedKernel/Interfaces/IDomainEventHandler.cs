using MediatR;
namespace FPTS.FIT.BDRD.BuildingBlocks.SharedKernel.Interfaces
{
    public interface IDomainEventHandler<T> : INotificationHandler<T> where T : BaseDomainEvent 
    {
    }
}
