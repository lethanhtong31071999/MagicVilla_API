using MagicVilla_Web.Models.Dto.ApplicationUserDTO;

namespace MagicVilla_Web.Services.IServices
{
    public interface IAuthService : IBaseService
    {
        Task<T> Login<T>(LoginRequestDTO obj);
        Task<T> Register<T>(RegisterRequestDTO obj);

    }
}
