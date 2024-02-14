using AutoMapper;
using RabbitSampleApis.SharedModels;
using RabbitSampleApis.SharedModels.Dto;
using RabbitSampleApis.UserService.Model;

namespace RabbitSampleApis.UserService.Mapper;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<IUserCreated, User>();
        CreateMap<IUserUpdated, User>();
        CreateMap<User, GetUserResponseDto>();
    }
}