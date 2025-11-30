using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // Para System.Text.Json
using Newtonsoft.Json;                // Para Newtonsoft (por si acaso)

namespace Api.Models
{
    public class Profile
    {
        [Key]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

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

        // --- CAMBIO AQUÍ: AGREGAR JSON IGNORE ---
        // Como tu base de datos no tiene estas columnas, no debemos enviarlas.
        
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        // --- ESTO YA LO TENÍAS IGNORADO, DÉJALO ASÍ ---
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public List<ProfileSkill>? ProfileSkills { get; set; }
    }
}