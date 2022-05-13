using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using FPTS.FIT.BDRD.BuildingBlocks.Logger.Kafka.Configs;
using System.Text.RegularExpressions;

namespace FPTS.FIT.BDRD.BuildingBlocks.Logger.Kafka
{
    /// <summary>
    /// Class logger: khi dùng ILogger, thông tin về log sẽ được đẩy lên kafka
    /// </summary>
    public class LoggerKafka : ILogger
    {
        private readonly string _name;
        private readonly LoggerKafkaConfiguration _config;
        private readonly LoggerKafkaProducer _producer;

        public LoggerKafka(LoggerKafkaProducer producer, LoggerKafkaConfiguration config, string name)
        {
            _name     = name;
            _config   = config;
            _producer = producer;
        }

        private LogLevel GetLogLevelByName(string name)
        {
            LogLevel logLevel = LogLevel.None;
            if (!string.IsNullOrEmpty(name))
            {
                switch (name.ToLower())
                {
                    case "debug":
                        logLevel = LogLevel.Debug;
                        break;
                    case "trace":
                        logLevel = LogLevel.Trace;
                        break;
                    case "info":
                    case "information":
                        logLevel = LogLevel.Information;
                        break;
                    case "warning":
                    case "warn":
                        logLevel = LogLevel.Warning;
                        break;
                    case "error":
                    case "err":
                        logLevel = LogLevel.Error;
                        break;
                    case "crit":
                    case "critical":
                        logLevel = LogLevel.Critical;
                        break;
                }
            }
            return logLevel;
        }

        private string GetLogLevelLongName(LogLevel logLevel)
            => logLevel switch
            {
                LogLevel.Trace       => "trace",
                LogLevel.Debug       => "debug",
                LogLevel.Information => "information",
                LogLevel.Warning     => "warning",
                LogLevel.Error       => "error",
                LogLevel.Critical    => "critical",
                _                    => throw new ArgumentOutOfRangeException(nameof(logLevel))
            };

        private string GetLogLevelShortName(LogLevel logLevel)
            => logLevel switch
            {
                LogLevel.Trace       => "trace",
                LogLevel.Debug       => "debug",
                LogLevel.Information => "info",
                LogLevel.Warning     => "warn",
                LogLevel.Error       => "err",
                LogLevel.Critical    => "crit",
                _                    => throw new ArgumentOutOfRangeException(nameof(logLevel))
            };

        private bool CheckMatchLogger(string logger, string loggerPattern)
        {
            bool endsWithAst = false;
            loggerPattern    = loggerPattern.Replace(".", "\\.");
            if (loggerPattern.EndsWith("*"))
            {
                loggerPattern = loggerPattern.Substring(0, loggerPattern.Length - 1);
                endsWithAst   = true;
            }
            loggerPattern = loggerPattern.Replace("*", "\\w+");
            if (endsWithAst)
            {
                loggerPattern += ".*";
            }
            loggerPattern = "^" + loggerPattern + "$";
            return Regex.IsMatch(logger, loggerPattern);
        }

        private string GetLogContent(LogLevel logLevel, string message, Target target)
        {
            var content = target.LogTemplate;
            if (!string.IsNullOrEmpty(content))
            {
                content = Regex.Replace(content, "{level[^}]*}", GetLogLevelName(logLevel, target.LogTemplateFormat.LevelNameFormat));
                content = Regex.Replace(content, "{message}", message);
                content = Regex.Replace(content, "{date[^}]*}", DateTime.Now.ToString(target.LogTemplateFormat.DatetimeFormat));
                content = Regex.Replace(content, "{logger}", _name);
                return content;
            }
            return message;
        }

        /// <summary>
        /// Lấy ra danh sách các Kafka topic theo logLevel mà sẽ được publish đến
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        private IEnumerable<Target> GetTargets(LogLevel logLevel)
        {
            var targets = new List<Target>();
            if ((_config.Rules != null && _config.Rules.Any()) && (_config.Targets != null && _config.Targets.Any()))
            {
                short logLv = (short)logLevel;
                foreach (var r in _config.Rules)
                {
                    short minLogLv = (short)GetLogLevelByName(r.MinLevel);
                    if (minLogLv == 6)
                    {
                        minLogLv = -1;
                    }
                    short maxLogLv = (short)GetLogLevelByName(r.MaxLevel);
                    if (logLv >= minLogLv && logLv <= maxLogLv && CheckMatchLogger(_name, r.Logger))
                    {
                        try
                        {
                            targets.Add(_config.Targets[r.WriteTo]);
                        }
                        catch { }
                    }
                }
            }
            return targets;
        }

        private string GetLogLevelName(LogLevel logLevel, LogLevelNameFormat levelNameFormat)
        {
            string level = GetLogLevelLongName(logLevel);
            if (levelNameFormat.TruncateShort > 0)
            {
                level = level.Length <= levelNameFormat.TruncateShort ? level : level.Substring(0, levelNameFormat.TruncateShort);
            }
            else
            {
                if (levelNameFormat.TruncateString.Equals("short"))
                {
                    level = GetLogLevelShortName(logLevel);
                }
            }
            if (levelNameFormat.Uppercase)
            {
                level = level.ToUpper();
            }
            return level;
        }

        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel) || _producer == null)
            {
                return;
            }
            if (_config.EventId == 0 || _config.EventId == eventId.Id)
            {
                var targets = GetTargets(logLevel);
                foreach (var t in targets)
                {
                    var logContent = GetLogContent(logLevel, formatter(state, exception), t);
                    _producer.Produce(new Message<Null, string> { Value = logContent }, t.Topic, t.Partition);
                }
            }
        }
    }
}
