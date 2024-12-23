using DataAccess.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utils.Mapper;

namespace Messaging.RabbitMq.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddAutoMapper(typeof(AppMappingProfile));
    }
}