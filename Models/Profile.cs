using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models
{
    public class Profile
    {
        [Key]
        [JsonPropertyName("id")]
        // CLAVE: "WhenWritingNull". Enviamos null desde C#, Supabase genera el UUID.
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Guid? Id { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("position")]
        public string Position { get; set; } = string.Empty;

        [JsonPropertyName("department_id")]
        public int? DepartmentId { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("is_available_for_change")]
        public bool IsAvailableForChange { get; set; }

        // --- SKILLS ---
        [JsonPropertyName("profile_skills")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ProfileSkill>? ProfileSkills { get; set; }
    }
}