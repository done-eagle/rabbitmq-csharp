using System.Text.Json;
using AutoMapper;
using RabbitSampleApis.Consumer.Context;
using RabbitSampleApis.Consumer.Model;
using RabbitSampleApis.Helper.Dto;
using RabbitSampleApis.Helper.RabbitMq;

namespace RabbitSampleApis.Consumer;

public class Program
{
    static void Interface()
    {
        Console.Write("\n>Меню программы<\n");
        Console.Write("(1)Добавить пользователя\n");
        Console.Write("(2)Получить пользователя\n");
        Console.Write("(3)Обновить пользователя\n");
        Console.Write("(4)Удалить пользователя\n");
        Console.Write("(5)Выход\n");
    }
    
    public static async Task Main(string[] args)
    {
        var rabbitMqListener = new RabbitMqListener();
        var message = await rabbitMqListener.ExecuteAsync();
        int menu;
        
        do
        {
            Interface();
            Console.Write("\nВыберите действие: ");
            menu = Convert.ToInt32(Console.ReadLine());
            Console.Write("\n");
            
            switch (menu)
            {
                case 1:
                    Console.Clear();
                    AddUser(message);
                    break;
                case 2:
                    Console.Clear();
                    GetUserById(message);
                    break;
                case 3:
                    Console.Clear();
                    UpdateUser(message);
                    break;
                case 4:
                    Console.Clear();
                    DeleteUser(message);
                    break;
            }
            
        } while (menu != 5);
    }

    private static void AddUser(string message)
    {
        var userDto = JsonSerializer.Deserialize<AddUserRequestDto>(message);
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AddUserRequestDto, User>();
        });
        
        var mapper = config.CreateMapper();
        var user = mapper.Map<User>(userDto);
        using (var db = new AppDbContext())
        {
            db.Users.Add(user);
            db.SaveChanges();
        }
        Console.WriteLine("User is added to DB!");
    }

    private static void GetUserById(string message)
    {
        var userDto = JsonSerializer.Deserialize<GetUserRequestDto>(message);
        var user = new User();
        using (var db = new AppDbContext())
        {
            if (userDto != null)
                user = db.Users.Find(userDto.Id);
        }
        Console.WriteLine(user?.ToString());
    }

    private static void UpdateUser(string message)
    {
        var userDto = JsonSerializer.Deserialize<UpdateUserRequestDto>(message);
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UpdateUserRequestDto, User>();
        });
        var mapper = config.CreateMapper();
        var user = mapper.Map<User>(userDto);
        
        using var db = new AppDbContext();
        db.Users.Update(user);
        db.SaveChanges();
        Console.WriteLine("User is updated in DB!");
    }

    private static void DeleteUser(string message)
    {
        var userDto = JsonSerializer.Deserialize<DeleteUserRequestDto>(message);
        using var db = new AppDbContext();
        if (userDto != null)
        {
            var user = db.Users.Find(userDto.Id);
            db.Users.Remove(user);
            db.SaveChanges();
        }
        Console.WriteLine("User is deleted from DB!");
    }
}