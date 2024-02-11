namespace RabbitSampleApis.Consumer.Model;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    public override string ToString()
    {
        return $"User(Id: {Id}, Name: {Name}, Email: {Email})";
    }
}