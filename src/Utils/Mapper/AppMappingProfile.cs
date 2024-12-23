using AutoMapper;
using Entities;
using UseCases.Dto;

namespace Utils.Mapper;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<CreateUserDto, User>();
        CreateMap<User, UserDto>();
    }
}