using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MySecureBackend.WebApi.Models
{
    public class Level2D
    {
        [Required]
        public Guid Id { get; set; }
        [Required, MaxLength(25)]
        public string Name { get; set; }
        [Range(50,750)]
        public int MaxLenght { get; set; }
        [Range(50,750)]
        public int MaxHeight { get; set; }
        [ValidateNever]
        public string UserID { get; set; } 
    }
}
