namespace HiLoGame.Api.Requests;

public class StartGameResponse
{
    public Guid GameId { get; set; }
    public bool IsGameStarted { get; set; }
}
