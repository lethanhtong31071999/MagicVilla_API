using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto.ApplicationUserDTO;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IUserRepo : IRepository<LocalUser>
    {
        public Task<bool> isUniqueUser(string username);
        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        public Task<RegisterResponseDTO> Register(RegisterRequestDTO registerRequestDTO);
        public Task<LocalUser> Update(UserDto userDto);
    }
}
