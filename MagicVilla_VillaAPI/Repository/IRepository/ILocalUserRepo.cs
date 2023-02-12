using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto.ApplicationUserDTO;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface ILocalUserRepo : IRepository<LocalUser>
    {
        public Task<bool> IsUniqueUser(string username);
        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        public Task<RegisterResponseDTO> Register(RegisterRequestDTO registerRequestDTO);
    }
}
