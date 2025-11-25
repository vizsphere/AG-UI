using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AG_UI_Server.Model
{
    public class Speaker
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("bio")]
        public string Bio { get; set; } = string.Empty;

        [JsonPropertyName("webSite")]
        public string WebSite { get; set; } = string.Empty;
    }
}
