using System;
using System.Text.Json.Serialization;

namespace Api.Models
{
    public class SlaRecords
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sla_metric_id")]
        public int SlaMetricId { get; set; } 

        [JsonPropertyName("profile_id")]
        public Guid ProfileId { get; set; } 
        [JsonPropertyName("record_date")]
        public DateTime RecordDate { get; set; }

        [JsonPropertyName("actual_percentage")]
        public decimal ActualPercentage { get; set; } 

        [JsonPropertyName("details")]
        public string Details { get; set; } = string.Empty;

        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }
    }
}