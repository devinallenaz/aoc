using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day22 : Solver
{
    public override int Day => 22;
    public override object ExpectedOutput1 => 6032;

    public override object Solve1(string input)
    {
        var character = CharacterOnMapFromData(input.Split("@").Last());
        character.DoAllMoves();
        return (int)(1000 * character.CurrentTile.Coordinates.y + 4 * character.CurrentTile.Coordinates.x + character.CurrentFacing.Value);
    }
    
    public override object ExpectedOutput2 => 6032;

    public override object Solve2(string input)
    {
        var parts = input.Split("@").ToArray();
        var character = CharacterOnMapFromData(parts[1], int.Parse(parts[0]));
        character.DoAllMoves();
        return (int)(1000 * character.CurrentTile.Coordinates.y + 4 * character.CurrentTile.Coordinates.x + character.CurrentFacing.Value);
    }
    
    private static char[] Digits { get; } = new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

    public static Character CharacterOnMapFromData(string input, int? cubeSize = null)
    {
        var sections = input.SplitSections().ToArray();

        var moves = MovesFromMoveData(sections[1]);
        var startingTile = MapFromMapData(sections[0], cubeSize);
        return new Character(startingTile, moves);
    }

    private static List<string> MovesFromMoveData(string moveString)
    {
        var currentString = "";
        List<string> moves = new();
        for (var i = 0; i < moveString.Length; i++)
        {
            if (Digits.Contains(moveString[i]))
            {
                currentString += moveString[i];
            }
            else
            {
                moves.Add(currentString);
                moves.Add(moveString[i].ToString());
                currentString = "";
            }
        }

        moves.Add(currentString);
        return moves.NonEmpty().ToList();
    }

    private static Tile MapFromMapData(string moveString, int? cubeSize)
    {
        var lines = moveString.SplitLines().ToArray();
        Tile? firstInLine = null;
        Tile? previous = null;
        Tile? startingTile = null;
        List<List<Tile>> columns = new List<List<Tile>>();
        var allTiles = new List<Tile>();
        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                if (columns.Count() == x)
                {
                    columns.Add(new List<Tile>());
                }

                switch (line[x])
                {
                    case '.':
                    {
                        var tile = new Tile((x + 1, y + 1), false);
                        allTiles.Add(tile);
                        if (startingTile == null)
                        {
                            startingTile = tile;
                        }

                        if (firstInLine == null)
                        {
                            firstInLine = tile;
                        }

                        if (previous != null)
                        {
                            previous.SetRightNeighbor(tile);
                            tile.SetLeftNeighbor(previous);
                        }

                        previous = tile;
                        columns[x].Add(tile);
                        break;
                    }

                    case '#':
                    {
                        var tile = new Tile((x + 1, y + 1), true);
                        allTiles.Add(tile);
                        if (startingTile == null)
                        {
                            startingTile = tile;
                        }

                        if (firstInLine == null)
                        {
                            firstInLine = tile;
                        }

                        if (previous != null)
                        {
                            previous.SetRightNeighbor(tile);
                            tile.SetLeftNeighbor(previous);
                        }

                        previous = tile;
                        columns[x].Add(tile);
                        break;
                    }
                }
            }

            if (previous != null && firstInLine != null)
            {
                previous.SetRightNeighbor(firstInLine);
                firstInLine.SetLeftNeighbor(previous);
            }

            previous = null;
            firstInLine = null;
        }

        foreach (var column in columns)
        {
            column.First().SetUpNeighbor(column.Last());
            column.Last().SetDownNeighbor(column.First());
            foreach (var tile in column)
            {
                if (previous != null)
                {
                    tile.SetUpNeighbor(previous);
                    previous.SetDownNeighbor(tile);
                }

                previous = tile;
            }

            previous = null;
        }

        if (cubeSize == 50)
        {
            for (int offset = 0; offset < 50; offset++)
            {
                //G O
                var tileA = allTiles.Single(t => t.Coordinates.x == 101 + offset && t.Coordinates.y == 50);
                var tileB = allTiles.Single(t => t.Coordinates.x == 100 && t.Coordinates.y == 51 + offset);
                tileA.SetDownNeighbor(tileB, c => c.TurnRight());
                tileB.SetRightNeighbor(tileA, c => c.TurnLeft());

                //Y R
                tileA = allTiles.Single(t => t.Coordinates.x == 51 + offset && t.Coordinates.y == 150);
                tileB = allTiles.Single(t => t.Coordinates.x == 50 && t.Coordinates.y == 151 + offset);
                tileA.SetDownNeighbor(tileB, c => c.TurnRight());
                tileB.SetRightNeighbor(tileA, c => c.TurnLeft());

                //O B
                tileA = allTiles.Single(t => t.Coordinates.x == 51 && t.Coordinates.y == 51 + offset);
                tileB = allTiles.Single(t => t.Coordinates.x == 1 + offset && t.Coordinates.y == 101);
                tileA.SetLeftNeighbor(tileB, c => c.TurnLeft());
                tileB.SetUpNeighbor(tileA, c => c.TurnRight());

                //G Y
                tileA = allTiles.Single(t => t.Coordinates.x == 150 && t.Coordinates.y == 1 + offset);
                tileB = allTiles.Single(t => t.Coordinates.x == 100 && t.Coordinates.y == 150 - offset);
                tileA.SetRightNeighbor(tileB, c =>
                {
                    c.TurnLeft();
                    c.TurnLeft();
                });
                tileB.SetRightNeighbor(tileA, c =>
                {
                    c.TurnLeft();
                    c.TurnLeft();
                });

                //W B
                tileA = allTiles.Single(t => t.Coordinates.x == 51 && t.Coordinates.y == 1 + offset);
                tileB = allTiles.Single(t => t.Coordinates.x == 1 && t.Coordinates.y == 150 - offset);
                tileA.SetLeftNeighbor(tileB, c =>
                {
                    c.TurnLeft();
                    c.TurnLeft();
                });
                tileB.SetLeftNeighbor(tileA, c =>
                {
                    c.TurnLeft();
                    c.TurnLeft();
                });

                //W R
                tileA = allTiles.Single(t => t.Coordinates.x == 51 + offset && t.Coordinates.y == 1);
                tileB = allTiles.Single(t => t.Coordinates.x == 1 && t.Coordinates.y == 151 + offset);
                tileA.SetUpNeighbor(tileB, c => c.TurnRight());
                tileB.SetLeftNeighbor(tileA, c => c.TurnLeft());

                //G R
                tileA = allTiles.Single(t => t.Coordinates.x == 101 + offset && t.Coordinates.y == 1);
                tileB = allTiles.Single(t => t.Coordinates.x == 1 + offset && t.Coordinates.y == 200);
                tileA.SetUpNeighbor(tileB);
                tileB.SetDownNeighbor(tileA);
            }
        }

        if (startingTile == null)
        {
            throw new InvalidOperationException("invalid initialization");
        }

        return startingTile;
    }

    private static void DrawMap(List<Tile> tiles)
    {
        Console.WriteLine();
        for (int y = 0; y < 200; y++)
        {
            for (int x = 0; x < 150; x++)
            {
                Console.ForegroundColor = tiles.FirstOrDefault(t => t.Coordinates == (x + 1, y + 1))?.DrawColor ?? ConsoleColor.Black;
                Console.Write("#");
            }

            Console.WriteLine();
        }
    }
}