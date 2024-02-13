using AutoMapper;
using RabbitSampleApis.SharedModels;
using RabbitSampleApis.UserService.Model;

namespace RabbitSampleApis.UserService.Mapper;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<IUserCreated, User>();
    }
}