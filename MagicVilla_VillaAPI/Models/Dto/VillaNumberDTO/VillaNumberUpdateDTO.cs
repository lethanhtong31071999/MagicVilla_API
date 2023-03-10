using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.Dto.VillaNumberDTO
{
    public class VillaNumberUpdateDTO
    {
        [Required]
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set; }
        public int VillaId { get; set; }
    }
}
