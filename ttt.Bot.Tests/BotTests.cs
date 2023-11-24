namespace ttt.Bot.Tests;

public class BotTests
{
    private Board _xWinningBoard = null!;
    private Board _oWinningBoard = null!;

    [SetUp]
    public void Setup()
    {
        _xWinningBoard = new Board(3);
        for (int i = 0; i < 5; i++)
            _xWinningBoard.TakeTurn(i + 1);

        _oWinningBoard = new Board(3);
        _oWinningBoard.TakeTurn(1);
        _oWinningBoard.TakeTurn(3);
        _oWinningBoard.TakeTurn(2);
        _oWinningBoard.TakeTurn(5);
        _oWinningBoard.TakeTurn(4);
    }

    [Test]
    public void Should_WinAsX()
    {
    }

    [Test]
    public void Should_TieAsO()
    {
        var bot = new Bot(Player.O);

        bot.TakeTurn(_xWinningBoard);
        Assert.That(_xWinningBoard.LastTakenCellNumber, Is.EqualTo(9));

        _xWinningBoard.TakeTurn(8);
        bot.TakeTurn(_xWinningBoard);
        Assert.That(_xWinningBoard.LastTakenCellNumber, Is.EqualTo(7));

        _xWinningBoard.TakeTurn(6);
        Assert.That(_xWinningBoard.GameEnded());
        Assert.That(_xWinningBoard.LastState, Is.EqualTo(GameState.Tie));
    }

    [Test]
    public void Should_TieAsX()
    {
    }

    [Test]
    public void Should_WinAsO()
    {
        var bot = new Bot(Player.O);

        bot.TakeTurn(_oWinningBoard);

        Assert.That(_oWinningBoard.LastTakenCellNumber, Is.EqualTo(7));
        Assert.That(_oWinningBoard.GameEnded());
        Assert.That(_oWinningBoard.LastState, Is.EqualTo(GameState.WinO));
    }
}