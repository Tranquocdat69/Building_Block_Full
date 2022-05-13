using Confluent.Kafka;
using FPTS.FIT.BDRD.BuildingBlocks.EventBus.Core;
using FPTS.FIT.BDRD.BuildingBlocks.EventBus.Kafka.Configurations;

namespace FPTS.FIT.BDRD.BuildingBlocks.EventBus.Kafka
{
    public class KafkaSubcriber<TKey, TValue> : ISubcriber<ConsumerData<TKey, TValue>>
    {
        private readonly ConsumerConfig _consumerConfig;

        public KafkaSubcriber(ConsumerBuilderConfiguration config)
        {
            _consumerConfig = new ConsumerConfig
            {
                GroupId           = config.GroupId,
                BootstrapServers  = config.BootstrapServers,
                EnableAutoCommit  = config.EnableAutoCommit,
                SessionTimeoutMs  = config.SessionTimeoutMs,
                QueuedMinMessages = config.QueuedMinMessages,
            };
        }

        /// <summary>
        /// Consume message từ kafka và thực hiện action delegate của message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        public void Consume(ConsumerData<TKey, TValue> message, CancellationToken cancellationToken = default)
        {
            using (var consumer = new ConsumerBuilder<TKey, TValue>(_consumerConfig).Build())
            {
                AssignOrSubscribeTopic(consumer, message.Topic, message.Partition, message.Offset);
                while (!cancellationToken.IsCancellationRequested)
                {
                    ConsumeResult<TKey, TValue>? record = consumer.Consume(TimeSpan.FromSeconds(1));
                    message.ConsumeResultAction(record);
                }
            }
        }

        /// <summary>
        /// Kiểm tra nếu offset >= 0 thì consumer bắt đầu từ offet
        /// </summary>
        /// <param name="consumer"></param>
        /// <param name="topic"></param>
        /// <param name="partition"></param>
        /// <param name="offset"></param>
        private void AssignOrSubscribeTopic(IConsumer<TKey, TValue> consumer, string topic, int partition = -1, long offset = -1)
        {
            if (partition >= 0)
            {
                if (offset >= 0)
                {
                    consumer.Assign(new TopicPartitionOffset(new TopicPartition(topic, partition), new Offset(offset)));
                }
                else
                {
                    consumer.Assign(new TopicPartition(topic, partition));
                }
            }
            else
            {
                consumer.Subscribe(topic);
            }
        }
    }
}
