using System.Text.Json.Serialization;

namespace Api.Models
{
    public class VacancySkill
    {
        [JsonPropertyName("vacancy_id")]
        public int VacancyId { get; set; }

        [JsonPropertyName("skill_id")]
        public int SkillId { get; set; }

        // --- AGREGAMOS LA PROPIEDAD FALTANTE ---
        [JsonPropertyName("grado")]
        public int Grado { get; set; }
    }
}