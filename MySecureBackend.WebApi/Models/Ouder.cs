using System.ComponentModel.DataAnnotations;

namespace MySecureBackend.WebApi.Models
{
    public class Ouder
    {
        [Required]
        public Guid OuderID { get; set; }
        [Required]
        public Guid KindID { get; set; }
        [Required]
        public string Naam { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
