using System.ComponentModel.DataAnnotations;

namespace MySecureBackend.WebApi.Models
{
    public class GameProgress
    {
        [Required]
        public Guid GameProgressID { get; set; }
        [Required]
        public float LevelProgress { get; set; }
        [Required]
        public int Points { get; set; }
    }
}
