using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MySecureBackend.WebApi.Models
{
    public class Patient
    {
        [Required]
        public Guid PatientID { get; set; }
        [Required]
        public string voornaam { get; set; }
        [Required]
        public string achternaam { get; set; }
        [Required]
        public int Leeftijd { get; set; }
        [ValidateNever]
        public string UserID { get; set; }
    }
}
