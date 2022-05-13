using Confluent.Kafka;
using FPTS.FIT.BDRD.BuildingBlocks.EventBus.Core;

namespace FPTS.FIT.BDRD.BuildingBlocks.EventBus.Kafka
{
    public class KafkaSubcriberService<TKey, TValue> : IKafkaSubcriberService<TKey, TValue>
    {
        private readonly ISubcriber<ConsumerData<TKey, TValue>> _subcriber;

        public KafkaSubcriberService(ISubcriber<ConsumerData<TKey, TValue>> subcriber)
        {
            _subcriber = subcriber;
        }
        public void StartConsumeTask(Action<ConsumeResult<TKey, TValue>?> action, string topic, long offset = -1, int partition = -1, CancellationToken cancellationToken = default)
        {
            ConsumerData<TKey, TValue> data = new ConsumerData<TKey, TValue>(action, topic, offset, partition);
            Task.Factory.StartNew(() =>
            {
                _subcriber.Consume(data, cancellationToken);
            }, TaskCreationOptions.LongRunning);
        }
    }
}
