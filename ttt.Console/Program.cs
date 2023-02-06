using ttt;
using ttt.Console;

int GetCellNumber(Board board, bool prompt = true)
{
    if (prompt)
    {
        IoTools.WriteMessage("Choose a cell:  ");
    }

    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
    var gotNumber = int.TryParse(Console.ReadLine(), out var number);
    while (!gotNumber || number < board.MinCellNumber || number > board.MaxCellNumber)
    {
        IoTools.WriteMessage(
            $"Please enter a number from {board.MinCellNumber} to {board.MaxCellNumber}: "
        );
        gotNumber = int.TryParse(Console.ReadLine(), out number);
    }

    return number;
}

void MarkCell(Board board)
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

void TakeTurn(Board board)
{
    var player = board.LastState switch
    {
        GameState.TurnO or GameState.Initial => "X",
        GameState.TurnX => "O",
        _ => ""
    };

    IoTools.WriteHeader($"Player {player}'s turn");
    IoTools.WriteBoard(board);
    MarkCell(board);
}


void Run()
{
    var board = new Board(3);
    while (!board.GameEnded())
    {
        TakeTurn(board);
    }

    var gameResult = board.LastState switch
    {
        GameState.WinX => "Player X won!",
        GameState.WinO => "Player O won!",
        GameState.Tie => "It's a tie!",
        _ => throw new InvalidGameStateException()
    };

    IoTools.WriteHeader("Game Over");
    IoTools.WriteBoard(board);
    IoTools.WriteMessage(gameResult);
}

Run();