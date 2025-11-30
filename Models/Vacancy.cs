using System;
using System.Collections.Generic;
using System.Text.Json.Serialization; // Importante para [JsonIgnore] y [JsonPropertyName]
using Newtonsoft.Json;                // Importante por si acaso

namespace Api.Models
{
    public class Vacancy
    {
        // 1. Ignoramos ID (Autoincremental)
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("department_id")]
        public int? DepartmentId { get; set; }

        // --- 2. EL ERROR ACTUAL: Ignoramos created_by ---
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [JsonPropertyName("created_by")]
        public Guid? CreatedBy { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        // 3. Ignoramos Fecha (Automática)
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        // 4. Ignoramos Lista (Relación externa)
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public List<VacancySkill>? VacancySkills { get; set; }
    }
}