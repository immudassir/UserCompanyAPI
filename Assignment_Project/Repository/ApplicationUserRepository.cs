using Assignment_Project.Data;
using Assignment_Project.Models.Dto;
using Assignment_Project.Models;
using Assignment_Project.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Assignment_Project.Repository
{
    public class ApplicationUserRepository : IApplicationUsersRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly string _secretKey;

        public ApplicationUserRepository(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            return !_db.ApplicationUsers.Any(x => x.UserName == username);
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _userManager.FindByNameAsync(loginRequestDTO.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password))
            {
                return new LoginResponseDTO { Token = "", ApplicationUser = null };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var loginResponseDTO = new LoginResponseDTO
            {
                Token = tokenHandler.WriteToken(token),
                ApplicationUser = _mapper.Map<ApplicationUserDTO>(user)
            };
            return loginResponseDTO;
        }

        public async Task<ApplicationUserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            var user = new ApplicationUser
            {
                UserName = registerationRequestDTO.UserName,
                Email = registerationRequestDTO.UserName,
                NormalizedEmail = registerationRequestDTO.UserName.ToUpper(),
                Name = registerationRequestDTO.Name
            };

            var result = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("customer"));
                }

                await _userManager.AddToRoleAsync(user, "admin");
                var userToReturn = await _userManager.FindByNameAsync(registerationRequestDTO.UserName);
                return _mapper.Map<ApplicationUserDTO>(userToReturn);
            }

            // Handle registration failure or validation errors here
            return new ApplicationUserDTO();
        }
    }
}
