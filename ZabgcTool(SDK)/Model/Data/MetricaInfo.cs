using Newtonsoft.Json;

namespace ZabgcTool_SDK_.Model.Data
{
    public class MetricaInfo
    {

        [JsonProperty("CountUser")]
        public int CountUser { get; set; }
        [JsonProperty("PercentDenided")]
        public double PercentDenided { get; set; }
        [JsonProperty("PageDepth")]
        public double PageDepth { get; set; }
        [JsonProperty("AvgDuration")]
        public double AvgDuration { get; set; }
        public MetricaInfo() { }

    }
}
