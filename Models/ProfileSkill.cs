using System;
using System.Text.Json.Serialization;

namespace Api.Models
{
    public class ProfileSkill
    {
        [JsonPropertyName("profile_id")]
        public Guid ProfileId { get; set; }

        [JsonPropertyName("skill_id")]
        public int SkillId { get; set; }
    }
}
