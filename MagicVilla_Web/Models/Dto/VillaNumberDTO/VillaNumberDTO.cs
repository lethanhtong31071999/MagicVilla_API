using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto.VillaNumberDTO
{
    public class VillaNumberDTO
    {
        [Required]
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set; }
        public int VillaId { get; set; }
        [ValidateNever]
        public MagicVilla_Web.Models.Dto.VillaDTO.VillaDTO Villa { get; set; }
    }
}
