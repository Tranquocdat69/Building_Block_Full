using Confluent.Kafka;
using FPTS.FIT.BDRD.BuildingBlocks.EventBus.Core;

namespace FPTS.FIT.BDRD.BuildingBlocks.EventBus.Kafka
{
    public class ConsumerData<TKey, TValue> : IMessage
    {
        public Action<ConsumeResult<TKey, TValue>?> ConsumeResultAction { get; init; }
        public string Topic { get; init; }
        public long Offset { get; init; }
        public int Partition { get; init; }

        public ConsumerData(Action<ConsumeResult<TKey, TValue>?> action, string topic, long offset = -1, int partition = -1)
        {
            ConsumeResultAction = action;
            Topic = topic;
            Offset = offset;
            Partition = partition;
        }
    }
}
