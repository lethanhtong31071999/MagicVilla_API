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
    public class UserRepo : Repository<LocalUser>, IUserRepo
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly byte[] _encodedSecretKey;
        public UserRepo(ApplicationDbContext db, IMapper mapper, IConfiguration configuration) : base(db)
        {
            _db = db;
            _mapper = mapper;
            _encodedSecretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("ApiSetting:SecretKey"));
        }
        public async Task<bool> isUniqueUser(string username)
        {
            var localUser = await base.Get(x => x.UserName == username, isTracked: false);
            return localUser == null;
        }

        public async Task<RegisterResponseDTO> Register(RegisterRequestDTO registerRequestDTO)
        {
            var localUser = _mapper.Map<LocalUser>(registerRequestDTO);
            await _db.LocalUsers.AddAsync(localUser);
            return _mapper.Map<RegisterResponseDTO>(localUser);
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var localUser = await base.Get(x => 
                x.UserName.ToLower() == loginRequestDTO.UserName.ToLower()
                && x.Password == loginRequestDTO.Password, isTracked: false);
            if (localUser == null) return null;

            // Generate Token if user exist in database
            var tokenDiscriptor = new SecurityTokenDescriptor()
            {
                Expires = DateTime.UtcNow.AddHours(3),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_encodedSecretKey), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, localUser.Id.ToString()),
                    new Claim(ClaimTypes.Role, localUser.Role)
                }),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDiscriptor);
            return new LoginResponseDTO
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDto>(localUser),
            };
        }

        public Task<LocalUser> Update(UserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
}
