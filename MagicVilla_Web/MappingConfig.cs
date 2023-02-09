using AutoMapper;
using MagicVilla_Web.Models.Dto.VillaDTO;
using MagicVilla_Web.Models.Dto.VillaNumberDTO;

namespace MagicVilla_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // Villa
            CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();

            // Villa Number
            CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();
            CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();
        }
    }
}
