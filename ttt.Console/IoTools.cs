namespace ttt.Console;

public static class IoTools
{
    public static void ClearLine()
    {
        for (int i = 0; i < 80; i++)
            System.Console.Write(" ");
    }

    public static void WriteHeader(string message)
    {
        System.Console.SetCursorPosition(0, 0);
        ClearLine();
        System.Console.SetCursorPosition(0, 0);
        System.Console.WriteLine(message);
    }

    public static void WriteMessage(string message)
    {
        System.Console.SetCursorPosition(0, System.Console.CursorTop - 1);
        ClearLine();
        System.Console.SetCursorPosition(0, System.Console.CursorTop);
        System.Console.Write(message);
    }

    public static void WriteBoard(Board board)
    {
        System.Console.WriteLine();
        var boardWidth = board.Size * 2 + 1;
        WriteBoardSeparator(boardWidth);
        for (int line = 0; line < board.Size; line++)
        {
            WriteBoardLine(board, line);
            WriteBoardSeparator(boardWidth);
        }

        System.Console.WriteLine();
        System.Console.WriteLine();
    }

    private static void WriteBoardSeparator(int length)
    {
        for (int i = 0; i < length / 2; i++)
            System.Console.Write("+-");
        System.Console.WriteLine("+");
    }

    private static void WriteBoardLine(Board board, int lineNumber)
    {
        System.Console.Write("|");
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
            System.Console.Write($"{cell}|");
        }

        System.Console.WriteLine();
    }
}