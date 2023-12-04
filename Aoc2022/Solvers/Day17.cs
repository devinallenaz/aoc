using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day17 : Solver
{
    public override int Day => 17;

    public override object ExpectedOutput1 => 3068L;

    public override object Solve1(string input)
    {
        var cave = new Rock((0, 0L), new[] { (0, 0L), (1, 0), (2, 0), (3, 0), (4, 0), (5, 0), (6, 0) });
        var shapeCycle = new Cycle<(int, long)[]>(new[] { Rock.HorizontalShape, Rock.PlusShape, Rock.LShape, Rock.VerticalShape, Rock.SquareShape });
        var moveCycle = new Cycle<char>(input.Trim().ToArray());
        for (int i = 0; i < 2022; i++)
        {
            cave = DropRock(cave, shapeCycle, moveCycle);
        }


        return cave.MaxY;
    }


    public override object ExpectedOutput2 => 1514285714288L;

    public override object Solve2(string input)
    {
        checked
        {
            var cave = new Rock((0, 0), new[] { (0, 0L), (1, 0L), (2, 0L), (3, 0L), (4, 0L), (5, 0L), (6, 0L) });
            var shapeCycle = new Cycle<(int, long)[]>(new[] { Rock.HorizontalShape, Rock.PlusShape, Rock.LShape, Rock.VerticalShape, Rock.SquareShape });
            var moveCycle = new Cycle<char>(input.Trim().ToArray());

            var (cycleHeight, cycleRocks, baseRocks, baseHeight, cycleEndCave) = DetectCycle(cave, shapeCycle, moveCycle);
            var remainingRocks = 1000000000000L - baseRocks;
            var remainingFullCycles = remainingRocks / cycleRocks;
            var heightAfterCycles = baseHeight + (cycleHeight * remainingFullCycles);
            var finalRocks = remainingRocks - (cycleRocks * remainingFullCycles);
            var cycleEndCaveHeight = cycleEndCave.MaxY;
            Rock caveAfterCycles = new Rock((0, heightAfterCycles), cycleEndCave.PointsCovered.Select(p => (p.x, p.y - cycleEndCaveHeight)).ToArray());
            for (int i = 0; i < finalRocks; i++)
            {
                caveAfterCycles = DropRock(caveAfterCycles, shapeCycle, moveCycle);
            }


            return caveAfterCycles.MaxY;
        }
    }


    private Rock DropRock(Rock cave, Cycle<(int, long)[]> shapeCycle, Cycle<char> moveCycle)
    {
        checked
        {
            var rock = new Rock((2, cave.MaxY + 4), shapeCycle.Next());
            var down = rock;
            while (!down.Overlaps(cave))
            {
                rock = down;
                switch (moveCycle.Next())
                {
                    case '<':
                        if (rock.MinX > 0)
                        {
                            var left = rock.MoveLeft();
                            if (!left.Overlaps(cave))
                            {
                                rock = left;
                            }
                        }

                        break;
                    case '>':
                        if (rock.MaxX < 6)
                        {
                            var right = rock.MoveRight();
                            if (!right.Overlaps(cave))
                            {
                                rock = right;
                            }
                        }

                        break;
                }

                down = rock.MoveDown();
            }

            var newPoints = cave.PointsCovered.Concat(rock.PointsCovered);
            var maxY = newPoints.Max(p => p.y);
            return new Rock((0, 0), newPoints.Where(p => p.y >= maxY - 100).ToArray());
        }
    }

    public (long height, long rocks, long baseRocks, long baseHeight, Rock cave) DetectCycle(Rock cave, Cycle<(int, long)[]> shapeCycle, Cycle<char> moveCycle)
    {
        checked
        {
            var rockCount = 0L;
            var rockCountOld = 0L;
            var height = 0L;
            var heightIncreaseCache = new List<(long rockCount, long heightIncrease, long rockIncrease)>();
            int? targetIndex = null;
            while (true)
            {
                if (rockCount > 10000 && shapeCycle.CurrentIndex == 0 && targetIndex == null)
                {
                    targetIndex = moveCycle.CurrentIndex;
                }

                if (moveCycle.CurrentIndex == targetIndex && shapeCycle.CurrentIndex == 0 && rockCount != 0)
                {
                    var heightIncrease = cave.MaxY - height;
                    var rockIncrease = rockCount - rockCountOld;
                    rockCountOld = rockCount;
                    height = cave.MaxY;
                    heightIncreaseCache.Add((rockCount, heightIncrease, rockIncrease));
                    if (rockCount > 10000)
                    {
                        for (int i = 15; i < heightIncreaseCache.Count(); i *= 2)
                        {
                            var secondCycleCandidate = heightIncreaseCache.OrderByDescending(h => h.rockCount).Take(i).ToArray();
                            var firstCycleCandidate = heightIncreaseCache.OrderByDescending(h => h.rockCount).Skip(i).Take(i).ToArray();
                            var baseCandidate = heightIncreaseCache.OrderByDescending(h => h.rockCount).Skip(i * 2).ToArray();
                            if (secondCycleCandidate.Sum(h => h.heightIncrease) == firstCycleCandidate.Sum(h => h.heightIncrease))
                            {
                                return (secondCycleCandidate.Sum(h => h.heightIncrease),
                                        secondCycleCandidate.First().rockCount - firstCycleCandidate.First().rockCount,
                                        baseCandidate.First().rockCount,
                                        baseCandidate.Sum(h => h.heightIncrease),
                                        cave
                                    );
                            }
                        }
                    }
                }

                rockCount++;
                cave = DropRock(cave, shapeCycle, moveCycle);
            }
        }
    }


    public void PrintCave(Rock cave)
    {
        for (long y = cave.MaxY; y >= 0; y--)
        {
            for (int x = 0; x < 7; x++)
            {
                if (cave.PointsCovered.Contains((x, y)))
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
            }

            Console.Write("\n");
        }

        Console.WriteLine();
    }
}