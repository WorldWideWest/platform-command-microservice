using System.ComponentModel.DataAnnotations;

namespace CommandService.Models.Entities{
    public class Platform{
        [Key]
        public Guid Id { get; set; }
        public Guid ExternalId { get; set; }
        public string Name { get; set; }
        public ICollection<Command> Commands { get; set; } = new List<Command>();
    }
}