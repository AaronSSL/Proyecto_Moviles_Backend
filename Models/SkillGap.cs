using System;
using System.Text.Json.Serialization;

namespace Api.Models
{
    public class SkillGap
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("profile_id")]
        public Guid ProfileId { get; set; } 

        [JsonPropertyName("skill_id")]
        public int SkillId { get; set; } 
        [JsonPropertyName("required_level")]
        public int RequiredLevel { get; set; } 

        [JsonPropertyName("actual_level")]
        public int ActualLevel { get; set; } 

        [JsonPropertyName("gap_size")]
        public int GapSize { get; set; } 

        [JsonPropertyName("is_critical")]
        public bool IsCritical { get; set; } // Si la habilidad es considerada crítica (como en el dashboard)

        [JsonPropertyName("detection_date")]
        public DateTime DetectionDate { get; set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }
    }
}