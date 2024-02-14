namespace RabbitSampleApis.SharedModels.Dto;

public class DeleteUserRequestDto
{
    public int Id { get; set; }
    public DeleteUserRequestDto(int id)
    {
        Id = id;
    }
}