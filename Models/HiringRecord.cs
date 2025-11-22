using System;
using System.Text.Json.Serialization;

namespace Api.Models
{
    public class HiringRecord
    {
        [JsonPropertyName("hiring_record_id")]
        public Guid HiringRecordId { get; set; } = Guid.NewGuid();

        [JsonPropertyName("vacancy_id")]
        public int VacancyId { get; set; }

        [JsonPropertyName("profile_id")]
        public Guid ProfileId { get; set; }

        [JsonPropertyName("hiring_date")]
        public DateTime HiringDate { get; set; }

        [JsonPropertyName("hiring_type")]
        public string HiringType { get; set; } = string.Empty; 
        
        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
    }
}