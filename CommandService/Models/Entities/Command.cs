namespace CommandService.Models.Entities{
    public class Command{
        public Guid Id { get; set; }
        public string HowTo { get; set; }
        public string CLI { get; set; }
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; }
    }
}