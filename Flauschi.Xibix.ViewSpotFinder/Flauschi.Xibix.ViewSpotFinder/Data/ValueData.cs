using System.Text.Json.Serialization;

namespace Flauschi.Xibix.ViewSpotFinder.Data
{
    public class ValueData
    {
        [JsonPropertyName("element_id")]
        public int ElementId { get; set; } = default!;

        public double Value { get; set; } = default!;
    }
}