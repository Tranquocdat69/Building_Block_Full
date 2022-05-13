namespace FPTS.FIT.BDRD.BuildingBlocks.Logger.Kafka.Configs
{
    public class LogLevelNameFormat
    {
        private string _truncateString = "long";
        public short TruncateShort { get; set; } = -1;
        public string TruncateString
        {
            get => _truncateString;
            set
            {
                switch (value)
                {
                    case "short":
                        _truncateString = value;
                        break;
                    default:
                        _truncateString = "long";
                        break;
                }
            }
        }
        public bool Uppercase { get; set; } = true;
    }
}
