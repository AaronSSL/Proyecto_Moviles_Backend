using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json; 

namespace Api.Models
{
    public class Vacancy
    {
        // ARREGLADO: Usamos el nombre completo para evitar el conflicto
        [JsonPropertyName("id")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] 
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("department_id")]
        public int? DepartmentId { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        // ARREGLADO: Nombre completo aquí también
        [JsonPropertyName("skills")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<VacancySkill>? VacancySkills { get; set; }
    }
}