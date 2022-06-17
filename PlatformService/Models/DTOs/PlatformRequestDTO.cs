using System.ComponentModel.DataAnnotations;

namespace PlatformService.Models.DTOs{
    public class PlatformRequestDTO{
        [Required]
        public string Name { get; set; }
        [Required]
        public string Publisher { get; set; }
        [Required]
        public string Cost { get; set; }
    }
}