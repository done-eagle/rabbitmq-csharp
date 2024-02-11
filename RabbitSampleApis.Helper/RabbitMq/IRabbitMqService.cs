namespace RabbitSampleApis.Helper.RabbitMq;

public interface IRabbitMqService
{
    void SendMessage(object obj);
    void SendMessage(string message);
}