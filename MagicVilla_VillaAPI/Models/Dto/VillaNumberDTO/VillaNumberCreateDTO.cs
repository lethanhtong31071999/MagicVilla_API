using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MagicVilla_VillaAPI.Models.Dto.VillaNumberDTO
{
    public class VillaNumberCreateDTO
    {
        [Required]
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set; }
        [Required]
        public int VillaId { get; set; }
    }
}
