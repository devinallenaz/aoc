using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Solvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aoc2022.Solvers;

public class Day13 : Solver
{
    public override int Day => 13;

    public override object ExpectedOutput1 => 13;

    public override object Solve1(string input)
    {
        var pairs = IntOrIntArrayPairsFromData(input);
        return pairs.WithIndex().Where((p) => p.item.First().CompareTo(p.item.Last()) <= 0).Sum(p => p.index + 1);
    }
    
    public override object ExpectedOutput2 => 140;

    public override object Solve2(string input)
    {
        var packets = IntOrIntArraysFromData(input);
        return packets.OrderBy(p => p).Select(p => p.ToString()).WithIndex().Where((t) => t.item == "[[2]]" || t.item == "[[6]]").Select(t => t.index + 1).Aggregate((i, j) => i * j);
    }
    
    
    public static IEnumerable<IEnumerable<IIntOrIntArray>> IntOrIntArrayPairsFromData(string input)
    {
        return input.SplitSections().Select(p => p.SplitLines().Select(IntOrIntArrayFromString)).ToArray();
    }

    public static IEnumerable<IIntOrIntArray> IntOrIntArraysFromData(string input)
    {
        return input.SplitLines().Select(IntOrIntArrayFromString).Append(IntOrIntArrayFromString("[[2]]")).Append(IntOrIntArrayFromString("[[6]]")).ToArray();
    }

    public static IIntOrIntArray IntOrIntArrayFromString(string input)
    {
        var obj = JsonConvert.DeserializeObject(input);
        if (obj is int i)
        {
            return new JustInt(i);
        }
        else
        {
            return IntOrIntArrayFromJsonObject(obj);
        }
    }

    public static IIntOrIntArray IntOrIntArrayFromJsonObject(object obj)
    {
        if (obj is JValue i)
        {
            return new JustInt(i.ToObject<int>());
        }
        else if (obj is JArray a)
        {
            return new IntArray(a.Select(IntOrIntArrayFromJsonObject));
        }
        else if (obj is int[] a2)
        {
            return new IntArray(a2.Select(i => new JustInt(i)));
        }

        throw new NotImplementedException(obj.GetType().Name);
    }
}