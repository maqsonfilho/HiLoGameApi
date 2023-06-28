namespace HiLoGame.Application.Dtos;

public class GuessDto : DtoBase
{
    public Guid PlayerId { get; set; }
    public Guid GameId { get; set; }
    public int GuessNumber { get; set; }
    public string Feedback { get; set; }
}
