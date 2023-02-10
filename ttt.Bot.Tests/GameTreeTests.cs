namespace ttt.Bot.Tests;

public class GameTreeTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Should_BuildValidTree()
    {
        var board = new Board(3);
        board.TakeTurn(1);
        board.TakeTurn(2);
        board.TakeTurn(3);
        board.TakeTurn(6);
        board.TakeTurn(5);
        board.TakeTurn(7);
        board.TakeTurn(4);

        var tree = GameTree.BuildFrom(board);

        Assert.That(tree.CurrentBoard, Is.EqualTo(board));
        Assert.That(tree.NextBoards.Count, Is.EqualTo(2));

        var firstChild = tree.NextBoards[0];
        var secondChild = tree.NextBoards[1];
        Assert.That(firstChild.CurrentBoard[8], Is.EqualTo(CellState.Nought));
        Assert.That(secondChild.CurrentBoard[8], Is.EqualTo(CellState.Empty));

        Assert.That(firstChild.NextBoards.Count, Is.EqualTo(1));
        Assert.That(secondChild.NextBoards.Count, Is.EqualTo(1));

        Assert.That(firstChild.NextBoards[0].CurrentBoard.GameEnded());
        Assert.That(secondChild.NextBoards[0].CurrentBoard.NextPlayer, Is.EqualTo(Player.X));
    }

    [Test]
    public void ShouldNot_BuildFromGameEnd()
    {
        var board = new Board(3);
        board.TakeTurn(1);
        board.TakeTurn(2);
        board.TakeTurn(3);
        board.TakeTurn(6);
        board.TakeTurn(5);
        board.TakeTurn(7);
        board.TakeTurn(4);
        board.TakeTurn(8);
        board.TakeTurn(9);

        var tree = GameTree.BuildFrom(board);

        Assert.That(tree.CurrentBoard.GameEnded());
        Assert.That(tree.NextBoards.Count, Is.EqualTo(0));
    }
}