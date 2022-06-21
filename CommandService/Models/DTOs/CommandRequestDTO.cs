using System.Text.Json.Serialization;

namespace CommandService.Models.DTOs{
    public class CommandRequestDTO
    {
        public string HowTo { get; set; }
        public string CLI { get; set; }
        [JsonIgnore]
        public Guid PlatformId { get; set; }
    }
}