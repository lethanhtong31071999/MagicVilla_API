using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto.VillaDTO
{
    public class VillaCreateDTO
    {
        [Required]
        [MaxLength(30)]
        [MinLength(1)]
        public string Name { get; set; }
        [Required]
        public double Rate { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public double Sqft { get; set; }
        [Required]
        public int Occupancy { get; set; }
        [ValidateNever]
        public string? ImageUrl { get; set; }
        [ValidateNever]
        public string? Amenity { get; set; }
    }
}
