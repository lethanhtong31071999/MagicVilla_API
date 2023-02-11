using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.Dto.VillaNumberDTO
{
    public class VillaNumberDTO
    {
        [Required]
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set; }
        public int VillaId { get; set; }
        [ValidateNever]
        public virtual MagicVilla_VillaAPI.Models.Dto.VillaDTO.VillaDTO Villa { get; set; }
    }
}
