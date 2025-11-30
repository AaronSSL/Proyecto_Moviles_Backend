using System;
using System.Text.Json.Serialization;

namespace Api.Models
{
    public class SlaMetric
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty; 

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("target_percentage")]
        public decimal TargetPercentage { get; set; } 

        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }
    }
}