using System.Diagnostics;

namespace Aoc2022.Helpers;

public enum RpsMove
{
    Rock = 1,
    Paper = 2,
    Scissors = 3
}

public enum RpsOutcome
{
    Lose,
    Draw,
    Win
}

public class RpsGame
{
    protected static RpsMove ParsePlay(string input)
    {
        switch (input)
        {
            case "A":
            case "X":
                return RpsMove.Rock;
            case "B":
            case "Y":
                return RpsMove.Paper;
            case "C":
            case "Z":
                return RpsMove.Scissors;
            default:
                throw new ApplicationException($"Unable to parse RpsMove: {input}");
        }
    }

    protected static int PointsForOutcome(RpsMove scorerMove, RpsMove opponentMove)
    {
        switch (scorerMove, opponentMove)
        {
            case (RpsMove.Rock, RpsMove.Rock):
            case (RpsMove.Paper, RpsMove.Paper):
            case (RpsMove.Scissors, RpsMove.Scissors):
                return 3;
            case (RpsMove.Paper, RpsMove.Rock):
            case (RpsMove.Scissors, RpsMove.Paper):
            case (RpsMove.Rock, RpsMove.Scissors):
                return 6;
            default:
                return 0;
        }
    }

    public RpsGame(int id, string enemyPlay, string myPlay)
    {
        this.Id = id;
        this.EnemyPlay = ParsePlay(enemyPlay);
        this.MyPlay = ParsePlay(myPlay);
    }

    public RpsGame(int id, RpsMove enemyPlay, RpsMove myPlay)
    {
        this.Id = id;
        this.EnemyPlay = enemyPlay;
        this.MyPlay = myPlay;
    }

    public int Id { get; }
    public RpsMove EnemyPlay { get; }
    public RpsMove MyPlay { get; }

    public int MyPoints
    {
        get { return PointsForOutcome(MyPlay, EnemyPlay) + (int)MyPlay; }
    }


    public int EnemyPoints
    {
        get { return PointsForOutcome(EnemyPlay, MyPlay) + (int)EnemyPlay; }
    }
}

public class RpsGameWithPreferredOutcome : RpsGame
{
    private static RpsOutcome ParseOutcome(string input)
    {
        switch (input)
        {
            case "X":
                return RpsOutcome.Lose;
            case "Y":
                return RpsOutcome.Draw;
            case "Z":
                return RpsOutcome.Win;
            default:
                throw new ApplicationException($"Could not parse RpsOutcome: {input}");
        }
    }

    private static RpsMove CalculateMyPlay(RpsMove enemyPlay, RpsOutcome preferredOutcome)
    {
        switch (enemyPlay, preferredOutcome)
        {
            case (_, RpsOutcome.Draw):
                return enemyPlay;
            case (RpsMove.Paper, RpsOutcome.Win):
            case (RpsMove.Rock, RpsOutcome.Lose):
                return RpsMove.Scissors;
            case (RpsMove.Rock, RpsOutcome.Win):
            case (RpsMove.Scissors, RpsOutcome.Lose):
                return RpsMove.Paper;
            case (RpsMove.Scissors, RpsOutcome.Win):
            case (RpsMove.Paper, RpsOutcome.Lose):
                return RpsMove.Rock;
            default:
                throw new NotImplementedException();
        }
    }

    public RpsGameWithPreferredOutcome(int id, string enemyPlay, string preferredOutcome) 
        : base(id, ParsePlay(enemyPlay), CalculateMyPlay(ParsePlay(enemyPlay), ParseOutcome(preferredOutcome)))
    {
    }
}