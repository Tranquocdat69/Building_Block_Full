using FPTS.FIT.BDRD.BuildingBlocks.SharedKernel.Interfaces;
using System.Text.Json.Serialization;

namespace FPTS.FIT.BDRD.BuildingBlocks.SharedKernel
#nullable disable
{
    public abstract class BaseRingEvent : IRingData
    { 
        public string RequestId { get; set; }
        [JsonIgnore]
        public int HandlerId { get; set; }
    }
}
