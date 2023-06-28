namespace HiLoGame.Api.Requests;

public class MakeGuessResponse
{
    public Guid GameId { get; set; }
    public string Feedback { get; set; }
}
