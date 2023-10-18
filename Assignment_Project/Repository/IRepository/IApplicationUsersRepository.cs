using Assignment_Project.Dtos;
using Assignment_Project.Models.Dto;

namespace Assignment_Project.Repository.IRepository;

public interface IApplicationUsersRepository
{
    bool IsUniqueUser(string username);
    Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
    Task<ApplicationUserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
}
