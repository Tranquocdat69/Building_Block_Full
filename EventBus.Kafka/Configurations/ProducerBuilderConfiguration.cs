namespace FPTS.FIT.BDRD.BuildingBlocks.EventBus.Kafka.Configurations
{
    public class ProducerBuilderConfiguration
    {
        public string BootstrapServers { get; set; } = "localhost:9092";
        public int QueueBufferingMaxMessages { get; set; } = 2000000;
        public int MessageSendMaxRetries { get; set; } = 3;
        public int RetryBackoffMs { get; set; } = 500;
    }
}
