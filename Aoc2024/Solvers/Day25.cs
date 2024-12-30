using AocHelpers;
using AocHelpers.Solvers;

public class Day25 : Solver
{
    public override int Day => 25;

    //Problem 1
    public override object ExpectedOutput1 => 3;

    private static bool KeyFitsLock(int[] key, int[] @lock)
    {
        for (var i = 0; i < key.Length; i++)
        {
            if (key[i] + @lock[i] > 5)
            {
                return false;
            }
        }

        return true;
    }

    public override object Solve1(string input)
    {
        var locksAndKeys = input.SplitSections().Select(s => s.To2dCharDictionary());
        var keys = new List<int[]>();
        var locks = new List<int[]>();
        foreach (var lockOrKey in locksAndKeys)
        {
            var shape = new int[5];
            for (var i = 0; i < 5; i++)
            {
                shape[i] = lockOrKey.Count(k => k.Key.x == i && k.Value == '#') - 1;
            }

            if (lockOrKey[(0, 0)] == '#')
            {
                locks.Add(shape);
            }
            else
            {
                keys.Add(shape);
            }
        }

        return keys.Sum(k => locks.Count(l => KeyFitsLock(k, l)));
    }

    //Problem 2
    public override object ExpectedOutput2 => base.ExpectedOutput2;

    public override object Solve2(string input)
    {
        return base.Solve2(input);
    }
}