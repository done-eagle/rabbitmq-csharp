using System.Reflection;
using DataAccess.Interfaces;
using DataAccess.PostgreSql;
using MassTransit;
using MediatR;
using Messaging.RabbitMq.Consumers;
using Messaging.RabbitMq.Extensions;
using Microsoft.EntityFrameworkCore;
using UseCases.Commands.CreateUser;
using UseCases.Dto;
using UseCases.Queries.GetById;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.AddDbContext<IDbContext, AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")!);
});

builder.Services.AddScoped<IRequestHandler<CreateUserCommand, int>, CreateUserCommandHandler>();
builder.Services.AddScoped<IRequestHandler<GetUserByIdQuery, UserDto>, GetUserByIdQueryHandler>();

var rabbitMqConfig = builder.Configuration.GetSection("RabbitMq");

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedConsumer>();
    x.AddConsumer<UserReceivedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqHost = rabbitMqConfig["HostName"];
        var username = rabbitMqConfig["Username"];
        var password = rabbitMqConfig["Password"];
        
        cfg.Host(new Uri($"rabbitmq://{rabbitMqHost}"), h =>
        {
            h.Username(username);
            h.Password(password);
        });
        
        cfg.ReceiveEndpoint("user-created-event", ep =>
        {
            ep.ConfigureConsumer<UserCreatedConsumer>(context);
        });
        
        cfg.ReceiveEndpoint("user-received-event", ep =>
        {
            ep.ConfigureConsumer<UserReceivedConsumer>(context);
        });
    });
});


builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
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