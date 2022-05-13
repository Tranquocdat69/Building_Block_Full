namespace FPTS.FIT.BDRD.BuildingBlocks.EventBus.Core
{
    public interface ISubcriber<T> where T : IMessage
    {
        public void Consume(T message, CancellationToken cancellationToken = default);
    }
}
