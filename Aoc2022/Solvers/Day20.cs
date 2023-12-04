using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Models;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day20 : Solver
{
    public override int Day => 20;


    public override object ExpectedOutput1 => 3L;

    public override object Solve1(string input)
    {
        var ring = RingFromData(input);
        ring.Mix();
        return ring.Find(0, 1000) + ring.Find(0, 2000) + ring.Find(0, 3000);
    }

    public override object ExpectedOutput2 => 1623178306L;

    public override object Solve2(string input)
    {
        var ring = RingFromData(input, 811589153L);
        for (var i = 0; i < 10; i++)
        {
            ring.Mix();
        }

        return ring.Find(0, 1000) + ring.Find(0, 2000) + ring.Find(0, 3000);
    }

    private static Ring<long> RingFromData(string input, long decryptionKey = 1)
    {
        var array = input.SplitLines().Select(s => long.Parse(s)).ToArray();
        return new Ring<long>(array, i => i * decryptionKey);
    }
}