using WebApplication1.Models;

namespace WebApplication1.Services;

public class BoardService
{
    public const string HumanMark = "X";
    public const string BotMark = "O";
    public const string HumanName = "Human";
    public const string BotName = "Bot";

    private const string EmptyCell = "";
    private const int CellCount = 9;
    private const int BotDelayMinSeconds = 3;
    private const int BotDelayMaxSeconds = 8;

    private readonly string[] _cells = new string[CellCount];
    private Turn _currentTurn;
    private DateTime? _botPlaysAt;

    public BoardService()
    {
        StartNewGame();
    }

    public void StartNewGame()
    {
        Array.Fill(_cells, EmptyCell);

        if (Random.Shared.Next(2) == 0)
        {
            _currentTurn = Turn.Human;
            _botPlaysAt = null;
        }
        else
        {
            _currentTurn = Turn.Bot;
            _botPlaysAt = DateTime.UtcNow + ScheduleBotDelay();
        }
    }

    public string[][] GetBoard()
    {
        return
        [
            [_cells[0], _cells[1], _cells[2]],
            [_cells[3], _cells[4], _cells[5]],
            [_cells[6], _cells[7], _cells[8]],
        ];
    }

    public Turn GetCurrentTurn()
    {
        return _currentTurn;
    }

    public bool TryPlayHumanMove(int cell)
    {
        if (_currentTurn != Turn.Human)
        {
            return false;
        }

        if (_cells[cell] != EmptyCell)
        {
            return false;
        }

        _cells[cell] = HumanMark;
        _currentTurn = Turn.Bot;
        _botPlaysAt = DateTime.UtcNow + ScheduleBotDelay();
        return true;
    }

    public void ApplyDueBotMove()
    {
        if (_botPlaysAt is null || DateTime.UtcNow < _botPlaysAt)
        {
            return;
        }

        PlayBotMove();
        _currentTurn = Turn.Human;
        _botPlaysAt = null;
    }

    private TimeSpan ScheduleBotDelay()
    {
        return TimeSpan.FromSeconds(Random.Shared.Next(BotDelayMinSeconds, BotDelayMaxSeconds + 1));
    }

    private void PlayBotMove()
    {
        var emptyCells = new List<int>();
        for (var cell = 0; cell < CellCount; cell++)
        {
            if (_cells[cell] == EmptyCell)
            {
                emptyCells.Add(cell);
            }
        }

        if (emptyCells.Count == 0)
        {
            return;
        }

        var chosenCell = emptyCells[Random.Shared.Next(emptyCells.Count)];
        _cells[chosenCell] = BotMark;
    }
}
