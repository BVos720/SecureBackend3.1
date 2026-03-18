using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MySecureBackend.WebApi.Models
{
    public class Ouder
    {
        
        public Guid OuderID { get; set; }
        [Required]
        public string Naam { get; set; }
        [ValidateNever]
        public string AccountID { get; set; }
    }
}
