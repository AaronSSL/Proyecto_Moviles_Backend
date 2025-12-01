using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models
{
    public class Sugerencia
    {
        [Key]
        [JsonPropertyName("id")]
        // IMPORTANTE: Usamos 'long?' porque en tu tabla es int8.
        // WhenWritingNull permite que Supabase asigne el autoincremental.
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Id { get; set; }

        [JsonPropertyName("user_id")]
        // Lo dejamos nullable. Si envías null, Supabase intentará usar el default: auth.uid()
        public Guid? UserId { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [JsonPropertyName("estado")]
        // Mapeamos el tipo 'estado_sugerencia' a string para evitar conflictos de serialización
        public string Estado { get; set; } = string.Empty;
        
        // Opcional: Si agregas la columna de fecha que sugerí antes
        // [JsonPropertyName("created_at")]
        // public DateTime? CreatedAt { get; set; }
    }
}