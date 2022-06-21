namespace CommandService.Models.DTOs{
    public class CommandResponseDTO
    {        
        public Guid Id { get; set; }   
        public string HowTo { get; set; }
        public string CLI { get; set; }
        public Guid PlatformId { get; set; }
    }
}