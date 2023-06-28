using System.ComponentModel.DataAnnotations;

namespace HiLoGame.Api.Requests;

public class GetGameResponse
{
    [Required(ErrorMessage = "Player name is missing")]
    public string PlayerName { get; private set; }

    public GetGameResponse(string playerName)
    {
        PlayerName = playerName;
    }
}
