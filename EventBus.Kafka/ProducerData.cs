using Confluent.Kafka;
using FPTS.FIT.BDRD.BuildingBlocks.EventBus.Core;

namespace FPTS.FIT.BDRD.BuildingBlocks.EventBus.Kafka
#nullable disable
{
    public class ProducerData<TKey, TValue> : IMessage
    {
        public TValue Value { get; private set; }
        public TKey Key { get; private set; }
        public string Topic { get; private set; }
        public int Partition { get; private set; }
        public List<Header> Headers { get; set; }

        public ProducerData(TValue value, TKey key, string topic, int partition = -1)
        {
            Value = value;
            Key = key;
            Topic = topic;
            Partition = partition;
            Headers = new List<Header>(); ;
        }
    }
}
