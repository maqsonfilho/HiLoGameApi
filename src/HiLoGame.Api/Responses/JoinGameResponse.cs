namespace HiLoGame.Api.Requests;

public class JoinGameResponse
{
    public Guid GameId { get; set; }
    public Guid PlayerId { get; internal set; }
    public string PlayerName { get; set; }
}
