using System.Text.RegularExpressions;
using AocHelpers;

namespace Aoc2023.Models;

public class PartSorter
{
    private static Regex sorterRegex = new Regex(@"(\w+){([\w:<>,]+),(\w+)}");

    private List<PartTest> Tests { get; }
    public string Label { get; }
    public string Fallback { get; }

    public PartSorter(string init)
    {
        var groups = sorterRegex.Match(init).Groups;
        this.Label = groups[1].Value;
        this.Fallback = groups[3].Value;
        this.Tests = groups[2].Value.SplitCommas().Select(s => new PartTest(s)).ToList();
    }

    public string Sort(Part part)
    {
        foreach (var test in this.Tests)
        {
            if (test.Test(part))
            {
                return test.Destination;
            }
        }

        return this.Fallback;
    }

    public IEnumerable<PartRange> Sort(PartRange partRange)
    {
        var partRangeToTest = partRange;

        foreach (var test in this.Tests)
        {
            var (pass, fail) = test.Test(partRangeToTest);
            pass.Destination = test.Destination;
            yield return pass;
            partRangeToTest = fail;
        }

        partRangeToTest.Destination = this.Fallback;
        yield return partRangeToTest;
    }

    private class PartTest
    {
        private static Regex testRegex = new Regex(@"([xmas])([<>])(\d+):(\w+)");

        private static bool GreaterThan(int partProperty, int value)
        {
            return partProperty > value;
        }

        private static bool LessThan(int partProperty, int value)
        {
            return partProperty < value;
        }

        private static int X(Part part)
        {
            return part.X;
        }

        private static int M(Part part)
        {
            return part.M;
        }

        private static int A(Part part)
        {
            return part.A;
        }

        private static int S(Part part)
        {
            return part.S;
        }

        public string Destination { get; }
        private Func<int, int, bool> Compare { get; }
        private Func<Part, int> PropertySelector { get; }
        private int CompareValue { get; }
        private bool Min { get; }
        private string Property { get; }

        public PartTest(string init)
        {
            var groups = testRegex.Match(init).Groups;
            this.Property = groups[1].Value;
            this.PropertySelector = this.Property switch
            {
                "x" => X,
                "m" => M,
                "a" => A,
                "s" => S,
                _ => throw new NotImplementedException(),
            };
            this.Compare = groups[2].Value switch
            {
                ">" => GreaterThan,
                "<" => LessThan,
                _ => throw new NotImplementedException(),
            };
            this.Min = this.Compare == GreaterThan;
            this.CompareValue = int.Parse(groups[3].Value);
            this.Destination = groups[4].Value;
        }

        public bool Test(Part part)
        {
            return this.Compare(PropertySelector(part), this.CompareValue);
        }

        public (PartRange pass, PartRange fail) Test(PartRange partRange)
        {
            var passCopy = partRange.Copy();
            var failCopy = partRange.Copy();
            if (this.Min)
            {
                switch (this.Property)
                {
                    case "x":
                        passCopy.MinX = Math.Max(passCopy.MinX, this.CompareValue + 1);
                        failCopy.MaxX = Math.Min(failCopy.MaxX, this.CompareValue);
                        break;
                    case "m":
                        passCopy.MinM = Math.Max(passCopy.MinM, this.CompareValue + 1);
                        failCopy.MaxM = Math.Min(failCopy.MaxM, this.CompareValue);
                        break;
                    case "a":
                        passCopy.MinA = Math.Max(passCopy.MinA, this.CompareValue + 1);
                        failCopy.MaxA = Math.Min(failCopy.MaxA, this.CompareValue);
                        break;
                    case "s":
                        passCopy.MinS = Math.Max(passCopy.MinS, this.CompareValue + 1);
                        failCopy.MaxS = Math.Min(failCopy.MaxS, this.CompareValue);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                switch (this.Property)
                {
                    case "x":
                        passCopy.MaxX = Math.Min(passCopy.MaxX, this.CompareValue - 1);
                        failCopy.MinX = Math.Max(failCopy.MinX, this.CompareValue);
                        break;
                    case "m":
                        passCopy.MaxM = Math.Min(passCopy.MaxM, this.CompareValue - 1);
                        failCopy.MinM = Math.Max(failCopy.MinM, this.CompareValue);
                        break;
                    case "a":
                        passCopy.MaxA = Math.Min(passCopy.MaxA, this.CompareValue - 1);
                        failCopy.MinA = Math.Max(failCopy.MinA, this.CompareValue);
                        break;
                    case "s":
                        passCopy.MaxS = Math.Min(passCopy.MaxS, this.CompareValue - 1);
                        failCopy.MinS = Math.Max(failCopy.MinS, this.CompareValue);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return (passCopy, failCopy);
        }
    }
}

public class Part
{
    private static Regex partRegex = new Regex(@"{x=(\d+),m=(\d+),a=(\d+),s=(\d+)}");
    public int X { get; }
    public int M { get; }
    public int A { get; }
    public int S { get; }
    public long Total => X + M + A + S;

    public Part(string init)
    {
        var groups = partRegex.Match(init).Groups;
        this.X = int.Parse(groups[1].Value);
        this.M = int.Parse(groups[2].Value);
        this.A = int.Parse(groups[3].Value);
        this.S = int.Parse(groups[4].Value);
    }
}

public class PartRange
{
    public long MinX { get; set; } = 1;
    public long MinM { get; set; } = 1;
    public long MinA { get; set; } = 1;
    public long MinS { get; set; } = 1;
    public long MaxX { get; set; } = 4000;
    public long MaxM { get; set; } = 4000;
    public long MaxA { get; set; } = 4000;
    public long MaxS { get; set; } = 4000;


    public string Destination { get; set; } = "in";
    public bool IsValid => this.MinX <= this.MaxX && this.MinA <= this.MaxA && this.MinM <= this.MaxM && this.MinS <= this.MaxS;
    public long PossibleVariations => (this.MaxX + 1 - this.MinX) * (this.MaxM + 1 - this.MinM) * (this.MaxA + 1 - this.MinA) * (this.MaxS + 1 - this.MinS);
    public PartRange Copy()
    {
        return new PartRange()
        {
            MinX = this.MinX,
            MinM = this.MinM,
            MinA = this.MinA,
            MinS = this.MinS,
            MaxX = this.MaxX,
            MaxM = this.MaxM,
            MaxA = this.MaxA,
            MaxS = this.MaxS,
        };
    }
}