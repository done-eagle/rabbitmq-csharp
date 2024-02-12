using AutoMapper;
using RabbitSampleApis.Consumer.Model;
using RabbitSampleApis.Helper.Dto;

namespace RabbitSampleApis.Consumer.Mapper;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<AddUserRequestDto, User>();
        CreateMap<UpdateUserRequestDto, User>();
    }
}