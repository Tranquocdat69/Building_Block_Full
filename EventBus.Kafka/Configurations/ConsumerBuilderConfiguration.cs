namespace FPTS.FIT.BDRD.BuildingBlocks.EventBus.Kafka.Configurations
{
    public class ConsumerBuilderConfiguration
    {
        public string GroupId { get; set; } = "groupid";
        public string BootstrapServers { get; set; } = "localhost:9092";
        public bool EnableAutoCommit { get; set; } = true;
        public int QueuedMinMessages { get; set; } = 1000000;
        public int SessionTimeoutMs { get; set; } = 6000;
    }
}
