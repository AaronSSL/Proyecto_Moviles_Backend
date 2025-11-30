    using System.Text.Json.Serialization;

    namespace Api.Models
    {
        public class Skill
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;
        }
    }
