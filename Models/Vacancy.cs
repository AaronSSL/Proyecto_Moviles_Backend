using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Api.Models
{
    public class Vacancy
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("department_id")]
        public int? DepartmentId { get; set; }

        [JsonPropertyName("created_by")]
        public Guid? CreatedBy { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        public List<VacancySkill>? VacancySkills { get; set; }
    }
}
