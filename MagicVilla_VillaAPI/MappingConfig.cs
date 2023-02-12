using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto.ApplicationUserDTO;
using MagicVilla_VillaAPI.Models.Dto.VillaDTO;
using MagicVilla_VillaAPI.Models.Dto.VillaNumberDTO;

namespace MagicVilla_VillaAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // Villa
            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();

            // VillaNumber
            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();

            // User
            CreateMap<LocalUser, UserDto>().ReverseMap();
            CreateMap<LocalUser, RegisterRequestDTO>().ReverseMap();
            CreateMap<LocalUser, RegisterResponseDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ReverseMap();
        }
    }
}
