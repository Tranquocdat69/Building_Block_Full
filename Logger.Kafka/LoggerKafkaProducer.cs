using Confluent.Kafka;

namespace FPTS.FIT.BDRD.BuildingBlocks.Logger.Kafka
{
    /// <summary>
    /// Class kafka producer phục vụ việc đẩy log lên kafka
    /// </summary>
    public class LoggerKafkaProducer : IDisposable
    {
        private readonly IProducer<Null, string> _producer;

        public LoggerKafkaProducer(IProducer<Null, string> producer)
        {
            _producer = producer;
        }

        public void Dispose()
        {
            _producer.Dispose();
        }

        public void Produce(Message<Null, string> message, string topic, int partiton = -1)
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
