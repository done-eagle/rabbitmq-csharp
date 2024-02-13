namespace RabbitSampleApis.SharedModels;

public interface IUserUpdated
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}