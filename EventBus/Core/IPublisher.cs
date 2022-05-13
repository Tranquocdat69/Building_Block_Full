namespace FPTS.FIT.BDRD.BuildingBlocks.EventBus.Core
{
    public interface IPublisher<T> : IDisposable where T : IMessage
    {
        public void Publish(T message);
    }
}
