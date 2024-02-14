using MassTransit;
using Microsoft.EntityFrameworkCore;
using RabbitSampleApis.UserService.Consumer;
using RabbitSampleApis.UserService.Context;
using RabbitSampleApis.UserService.Mapper;
using RabbitSampleApis.UserService.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<UserCreatedConsumer>();
builder.Services.AddScoped<UserReceivedConsumer>();
builder.Services.AddScoped<UserUpdatedConsumer>();
builder.Services.AddScoped<UserDeletedConsumer>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedConsumer>();
    x.AddConsumer<UserReceivedConsumer>();
    x.AddConsumer<UserUpdatedConsumer>();
    x.AddConsumer<UserDeletedConsumer>();
    
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("rmuser");
            h.Password("rmpassword");
        });

        cfg.ReceiveEndpoint("user-created-event", ep =>
        {
            ep.ConfigureConsumer<UserCreatedConsumer>(provider);
        });
        
        cfg.ReceiveEndpoint("user-received-event", ep =>
        {
            ep.ConfigureConsumer<UserReceivedConsumer>(provider);
        });
        
        cfg.ReceiveEndpoint("user-updated-event", ep =>
        {
            ep.ConfigureConsumer<UserUpdatedConsumer>(provider);
        });
        
        cfg.ReceiveEndpoint("user-deleted-event", ep =>
        {
            ep.ConfigureConsumer<UserDeletedConsumer>(provider);
        });
    }));
});

var app = builder.Build();
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var busControl = app.Services.GetRequiredService<IBusControl>();
busControl.StartAsync(lifetime.ApplicationStopping);

app.Run();