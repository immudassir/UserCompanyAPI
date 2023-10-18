using Assignment_Project.Dtos;
using Assignment_Project.Models;
using AutoMapper;

namespace Assignment_Project
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<Company, CompanyDTO>();
            CreateMap<CompanyDTO, Company>();

            

        }
    }
}
