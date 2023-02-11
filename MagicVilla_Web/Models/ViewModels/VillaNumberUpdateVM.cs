using MagicVilla_Web.Models.Dto.VillaNumberDTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.ViewModels
{
    public class VillaNumberUpdateVM
    {
        public VillaNumberUpdateDTO VillaNumber { get; set; }
        public VillaNumberUpdateVM()
        {
            VillaNumber = new VillaNumberUpdateDTO();
        }

        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; }

    }
}
