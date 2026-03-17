using System.ComponentModel.DataAnnotations;

namespace MySecureBackend.WebApi.Models
{
    public class Kind
    {
        [Required]
        public Guid KindID { get; set; }
        [Required]
        public Guid BehandelingID { get; set; }
        [Required]
        public string Naam { get; set; }
        [Required]
        public int Leeftijd { get; set; }
    }
}
