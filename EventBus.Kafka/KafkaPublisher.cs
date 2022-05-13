using Confluent.Kafka;
using FPTS.FIT.BDRD.BuildingBlocks.EventBus.Core;
using FPTS.FIT.BDRD.BuildingBlocks.EventBus.Kafka.Configurations;

namespace FPTS.FIT.BDRD.BuildingBlocks.EventBus.Kafka
{
    public class KafkaPublisher<TKey, TValue> : IPublisher<ProducerData<TKey, TValue>>
    {
        private readonly IProducer<TKey, TValue> _producer;

        public KafkaPublisher(ProducerBuilderConfiguration config)
        {
            ProducerConfig producerConfig = new ProducerConfig
            {
                BootstrapServers = config.BootstrapServers,
                QueueBufferingMaxMessages = config.QueueBufferingMaxMessages,
                MessageSendMaxRetries = config.MessageSendMaxRetries,
                RetryBackoffMs = config.RetryBackoffMs,
                LingerMs = 5
            };
            _producer = new ProducerBuilder<TKey, TValue>(producerConfig).Build();
        }
        public void Dispose()
        {
            _producer.Dispose();
        }

        /// <summary>
        /// lấy dữ liệu từ MessageData để tạo Message rồi 
        /// gọi ProduceToKafka()
        /// </summary>
        /// <param name="data"></param>
        public void Publish(ProducerData<TKey, TValue> data)
        {

            Message<TKey, TValue> message = new Message<TKey, TValue> { Value = data.Value };
            if (data.Key != null || typeof(TKey) == typeof(Null))
            {
                message.Key = data.Key;
            }
            if (data.Headers.Any())
            {
                foreach (var header in data.Headers)
                {
                    message.Headers.Add(header);
                }
            }
            Produce(message, data.Topic, data.Partition);
        }

        /// <summary>
        /// Đẩy message lên kafka
        /// </summary>
        /// <param name="message"></param>
        /// <param name="topic"></param>
        /// <param name="partiton"></param>
        private void Produce(Message<TKey, TValue> message, string topic, int partiton)
        {
            try
            {
                if (partiton < 0)
                {
                    _producer.Produce(topic, message);
                }
                else
                {
                    _producer.Produce(new TopicPartition(topic, partiton), message);
                }
            }
            catch (ProduceException<Null, string> e)
            {
                if (e.Error.Code == ErrorCode.Local_QueueFull)
                {
                    _producer.Poll(TimeSpan.FromSeconds(1));
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
