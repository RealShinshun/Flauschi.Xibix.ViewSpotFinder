using System.Text.Json.Serialization;

namespace Flauschi.Xibix.ViewSpotFinder.Data
{
    public class ElementData
    {
        public int Id { get; set; } = default!;

        [JsonPropertyName("nodes")]
        public int[] NodeIds { get; set; } = default!;
    }
}