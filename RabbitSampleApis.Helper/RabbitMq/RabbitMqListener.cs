using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitSampleApis.Helper.RabbitMq;

public class RabbitMqListener
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqListener()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "rmuser",
            Password = "rmpassword"
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(
            queue: "MyQueue", 
            durable: false, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null
        );
    }
    
    public Task<string> ExecuteAsync()
    {
        //stoppingToken.ThrowIfCancellationRequested();
        var tcs = new TaskCompletionSource<string>();
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            _channel.BasicAck(ea.DeliveryTag, false);
            
            tcs.SetResult(content);
        };
        _channel.BasicConsume("MyQueue", false, consumer);

        return tcs.Task;
    }
    
    public void Close()
    {
        _connection.Close();
    }
}