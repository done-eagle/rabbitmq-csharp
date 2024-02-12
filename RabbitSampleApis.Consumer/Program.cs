using System.Text.Json;
using AutoMapper;
using RabbitSampleApis.Consumer.Context;
using RabbitSampleApis.Consumer.Mapper;
using RabbitSampleApis.Consumer.Repository;
using RabbitSampleApis.Helper.RabbitMq;

namespace RabbitSampleApis.Consumer;

public class Program
{
    static void Interface()
    {
        Console.Write("\n>Menu<\n");
        Console.Write("(1)Create user\n");
        Console.Write("(2)Read user\n");
        Console.Write("(3)Update user\n");
        Console.Write("(4)Delete user\n");
        Console.Write("(5)Exit\n");
    }
    
    public static void Main(string[] args)
    {
        int menu;
        
        do
        {
            Interface();
            Console.Write("\nВыберите действие: ");
            menu = Convert.ToInt32(Console.ReadLine());
            
            var rabbitMqListener = new RabbitMqListener();
            var db = new AppDbContext();
        
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AppMappingProfile>();
            });
            var mapper = config.CreateMapper();
            IUserRepository repository = new UserRepository(db, mapper);

            try
            {
                switch (menu)
                {
                    case 1:
                        var createMessage = ListenAndCheckMessage(rabbitMqListener);
                        repository.AddUser(createMessage.Result);
                        Console.Clear();
                        Console.WriteLine("User is added to DB!");
                        break;
                    case 2:
                        var readMessage = ListenAndCheckMessage(rabbitMqListener);
                        var user = repository.GetUserById(readMessage.Result);
                        Console.Clear();
                        Console.WriteLine(user?.ToString());
                        break;
                    case 3:
                        var updateMessage = ListenAndCheckMessage(rabbitMqListener);
                        repository.UpdateUser(updateMessage.Result);
                        Console.Clear();
                        Console.WriteLine("User is updated in DB!");
                        break;
                    case 4:
                        var deleteMessage = ListenAndCheckMessage(rabbitMqListener);
                        repository.DeleteUser(deleteMessage.Result);
                        Console.Clear();
                        Console.WriteLine("User is deleted from DB!");
                        break;
                }
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Error: No messages");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (JsonException e)
            {
                Console.WriteLine("Error: Json is wrong");
            }
        } while (menu != 5);
    }

    private static async Task<string> ListenAndCheckMessage(RabbitMqListener rabbitMqListener)
    {
        var message = rabbitMqListener.ExecuteAsync();
        var timeoutTask = Task.Delay(TimeSpan.FromSeconds(1));
        var completedTask = await Task.WhenAny(message, timeoutTask);

        if (completedTask != message)
            throw new ArgumentException();

        return message.Result;
    }
}