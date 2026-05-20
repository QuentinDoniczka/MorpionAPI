namespace WebApplication1.DTOs;

public class BoardStateResponse
{
    public string[][] Board { get; set; } = [];
    public string CurrentTurn { get; set; } = string.Empty;
    public string XPlayer { get; set; } = string.Empty;
    public string OPlayer { get; set; } = string.Empty;
}
