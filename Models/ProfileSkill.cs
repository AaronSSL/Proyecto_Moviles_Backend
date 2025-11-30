using System;
using System.Text.Json.Serialization;

namespace Api.Models
{
    public class ProfileSkill
    {
        [JsonPropertyName("profile_id")]
        public Guid ProfileId { get; set; }

        [JsonPropertyName("skill_id")]
        public int SkillId { get; set; } // En tu DB es int8 (bigint), int o long funcionan bien.

        // --- AGREGAR ESTO ---
        [JsonPropertyName("grado")]
        public int Grado { get; set; } // En tu DB es int2, aquí usaremos int.
    }
}