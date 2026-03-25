using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MySecureBackend.WebApi.Models
{
    public class Kind
    {
        [Required]
        public Guid KindID { get; set; }
        [Required]
        public string Naam { get; set; }
        [Required]
        [Range(0, 18, ErrorMessage = "Leeftijd moet tussen 0 en 18 jaar zijn")]
        public int Leeftijd { get; set; }
        [ValidateNever] 
        public Guid OuderID { get; set; }
    }
}
