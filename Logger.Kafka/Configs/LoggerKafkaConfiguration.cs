namespace FPTS.FIT.BDRD.BuildingBlocks.Logger.Kafka.Configs
{
    public class LoggerKafkaConfiguration
    {
        public int EventId { get; set; }
        public IDictionary<string, Target> Targets { get; set; } = new Dictionary<string, Target> {
            {"Target1", new Target()}
        };

        public IEnumerable<Rule> Rules { get; set; } = new Rule[] { new Rule() };

        public string BootstrapServers { get; set; } = "localhost:9092";
        public string AppName { get; set; } = "";
    }

}
