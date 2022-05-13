using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FPTS.FIT.BDRD.BuildingBlocks.Logger.Kafka.Configs;

namespace FPTS.FIT.BDRD.BuildingBlocks.Logger.Kafka
{
    public static class LoggerKafkaExtensions
    {
        public static ILoggingBuilder AddKafkaLogger(this ILoggingBuilder builder, Action<LoggerKafkaConfiguration> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, LoggerKafkaProvider>();
            builder.Services.Configure<LoggerKafkaConfiguration>(configure);
            builder.Services.AddSingleton(sp =>
            {
                var bootstrapSevers = sp.GetRequiredService<IOptions<LoggerKafkaConfiguration>>().Value.BootstrapServers;
                ProducerConfig producerConfig = new ProducerConfig()
                {
                    BootstrapServers = bootstrapSevers,
                    QueueBufferingMaxMessages = 2000000,
                    RetryBackoffMs = 500,
                    MessageSendMaxRetries = 3,
                    LingerMs = 5
                };
                var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
                return new LoggerKafkaProducer(producer);
            });
            return builder;
        }
    }
}
