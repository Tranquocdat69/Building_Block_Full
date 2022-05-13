using Confluent.Kafka;

namespace FPTS.FIT.BDRD.BuildingBlocks.EventBus.Kafka
{
    public interface IKafkaSubcriberService<TKey, TValue>
    {
        void StartConsumeTask(Action<ConsumeResult<TKey, TValue>?> action, string topic, long offset, int partition, CancellationToken cancellationToken);
    }
}
