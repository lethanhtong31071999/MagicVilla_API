using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto.ApplicationUserDTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Repository
{
    public class LocalUserRepo : Repository<LocalUser>, ILocalUserRepo
    {
        private readonly ApplicationDbContext _db;
        private readonly byte[] _encodedSecretKey;
        private readonly IMapper _mapper;

        public LocalUserRepo(ApplicationDbContext db, IMapper mapper, IConfiguration configuration) : base(db)
        {
            _mapper = mapper;
            _db = db;
            _encodedSecretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("ApiSetting:SecretKey"));
        }

        public async Task<bool> IsUniqueUser(string username)
        {
            var localUser = await base.Get(x => x.UserName == username, isTracked: false);
            return localUser == null;
        }

        public async Task<RegisterResponseDTO> Register(RegisterRequestDTO registerRequestDTO)
        {
            var localUser = _mapper.Map<LocalUser>(registerRequestDTO);
            await base.Create(localUser);
            return _mapper.Map<RegisterResponseDTO>(localUser);
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var localUser = await base.Get(
                x => 
                    x.UserName == loginRequestDTO.UserName && x.Password == loginRequestDTO.Password,
                isTracked: false);
            if (localUser == null) return null;

            // Generate Token
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(_encodedSecretKey),
                    SecurityAlgorithms.EcdsaSha256Signature),
                Subject = new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, localUser.Id.ToString()),
                    new Claim(ClaimTypes.Role, localUser.Role),
                }),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new LoginResponseDTO()
            {
                User = _mapper.Map<UserDto>(localUser),
                Token = tokenHandler.WriteToken(token),
            };
        }
    }
}
