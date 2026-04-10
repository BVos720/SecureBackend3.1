using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MySecureBackend.WebApi.Models
{
    public class Settings
    {
        [Required]
        public Guid SettingsID { get; set; }
        [Required]
        public int Character { get; set; }
        public int? Taal { get; set; }
        public bool? Dyslexie { get; set; }
        public int? ColorTheme { get; set; }
        [ValidateNever]
        public Guid KindID { get; set; }
    }
}
