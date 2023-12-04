using AocHelpers;

namespace Aoc2023.Models;

public class CubeDrawGame
{
    public int Id { get; }

    public CubeDrawGame(string init) //init is an input string in the form: "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"
    {
        var parts = init.SplitAndTrim(":");
        this.Id = int.Parse(parts.First().SplitAndTrim().Last());
        this.Draws = parts.Last().Split(";").Select(s => new CubeDraw(s));
    }

    public IEnumerable<CubeDraw> Draws { get; }
}

public class CubeDraw
{
    public int Blue { get; }
    public int Red { get; }
    public int Green { get; }

    public CubeDraw(string init) //init is an input string in the form: "3 blue, 4 red"
    {
        foreach (var part in init.SplitCommas())
        {
            var parts = part.SplitAndTrim();
            switch (parts.Last())
            {
                case "blue":
                    this.Blue = int.Parse(parts.First());
                    break;

                case "green":
                    this.Green = int.Parse(parts.First());
                    break;

                case "red":
                    this.Red = int.Parse(parts.First());
                    break;
            }
        }
    }
}