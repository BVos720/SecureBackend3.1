using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MySecureBackend.WebApi.Models
{
    public class LevelProgress
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float LevelProgressValue { get; set; }
        [Required]
        public int Points { get; set; }
        [ValidateNever]
        public string UserID { get; set; }
    }
}
