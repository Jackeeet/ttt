namespace ttt.Console;

internal static class Program
{
    private static readonly Random rand = new();

    private static int GetCellNumber(Board board, bool prompt = true)
    {
        if (prompt)
        {
            IoTools.WriteMessage("Choose a cell:  ");
        }

        System.Console.SetCursorPosition(System.Console.CursorLeft - 1, System.Console.CursorTop);
        var gotNumber = int.TryParse(System.Console.ReadLine(), out var number);
        while (!gotNumber || number < board.MinCellNumber || number > board.MaxCellNumber)
        {
            IoTools.WriteMessage(
                $"Please enter a number from {board.MinCellNumber} to {board.MaxCellNumber}: "
            );
            gotNumber = int.TryParse(System.Console.ReadLine(), out number);
        }

        return number;
    }

    private static void MarkCell(Board board)
    {
        var cellNumber = GetCellNumber(board);
        var gameState = board.TakeTurn(cellNumber);
        while (gameState == GameState.Error)
        {
            IoTools.WriteMessage(
                $"Cell #{cellNumber} is already taken. Please choose a different cell:  "
            );
            cellNumber = GetCellNumber(board, prompt: false);
            gameState = board.TakeTurn(cellNumber);
        }
    }

    private static void TakeHumanTurn(Board board)
    {
        IoTools.WriteHeader($"Player {board.NextPlayer}'s turn");
        IoTools.WriteBoard(board);
        MarkCell(board);
    }

    private static void TakeBotTurn(Board board, Bot.Bot bot)
    {
        IoTools.WriteHeader($"Player {board.NextPlayer}'s turn");
        IoTools.WriteBoard(board);
        IoTools.WriteMessage($"Player {board.NextPlayer} is making a turn...");
        // Thread.Sleep(rand.Next(5, 11) * 1000);
        bot.TakeTurn(board);
        Thread.Sleep(2 * 1000);
    }

    public static void Main(string[] args)
    {
        var size = 3;
        IoTools.ClearScreen(size * 2 + 5);
        var board = new Board(size);
        var bot = new Bot.Bot(Player.O);

        while (!board.GameEnded())
        {
            TakeHumanTurn(board);
            if (board.GameEnded())
                break;
            TakeBotTurn(board, bot);
        }

        var gameResult = board.LastState switch
        {
            GameState.WinX or GameState.WinO => $"Player {board.NextPlayer} won!",
            GameState.Tie => "It's a tie!",
            _ => throw new InvalidGameStateException()
        };

        IoTools.WriteHeader("Game Over");
        IoTools.WriteBoard(board);
        IoTools.WriteMessage(gameResult);
    }
}