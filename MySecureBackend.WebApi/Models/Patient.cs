using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MySecureBackend.WebApi.Models
{
    public class Patient
    {
        [Required]
        public Guid GUID { get; set; }
        [Required]
        public int PrefabID { get; set; }
        [Required]
        public double PositionX { get; set; } 
        [Required]
        public double PositionY { get; set; }
        [Required, Range(1, 1000)]
        public double ScaleX { get; set; }
        [Required, Range(1, 1000)]
        public double ScaleY { get; set; }
        [Required]
        public double RotationZ { get; set; }
        [Required, Range(1,10)]
        public int SortingLayer { get; set; }
        [Required]
        public Guid EnviromentGUID { get; set; }

    }
}
