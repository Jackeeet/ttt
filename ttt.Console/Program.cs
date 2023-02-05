using ttt;

void DrawLine(int length)
{
    for (int i = 0; i < length / 2; i++)
        Console.Write("+-");
    Console.WriteLine("+");
}

void DrawBoardLine(Board board, int lineNumber)
{
    Console.Write("|");
    for (int index = 0; index < board.Size; index++)
    {
        var boardIndex = lineNumber * board.Size + index + 1;
        var cell = board[boardIndex] switch
        {
            CellState.Empty => boardIndex.ToString(),
            CellState.Cross => "X",
            CellState.Nought => "O",
            _ => throw new ArgumentException("Impossible cell state")
        };
        Console.Write($"{cell}|");
    }

    Console.WriteLine();
}

void DrawBoard(Board board)
{
    var boardWidth = board.Size * 2 + 1;
    DrawLine(boardWidth);
    for (int line = 0; line < board.Size; line++)
    {
        DrawBoardLine(board, line);
        DrawLine(boardWidth);
    }
}

var board = new Board(3);
DrawBoard(board);