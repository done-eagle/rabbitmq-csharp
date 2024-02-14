namespace RabbitSampleApis.SharedModels.Dto;

public class GetUserRequestDto
{
    public int Id { get; set; }
    public GetUserRequestDto(int id)
    {
        Id = id;
    }
}