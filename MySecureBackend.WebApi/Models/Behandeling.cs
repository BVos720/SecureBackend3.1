using System.ComponentModel.DataAnnotations;

namespace MySecureBackend.WebApi.Models
{
    public class Behandeling
    {
        [Required]
        public Guid BehandelingID { get; set; }
        [Required]
        public Guid GameProgressID { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public DateTime Datum { get; set; }
    }
}
