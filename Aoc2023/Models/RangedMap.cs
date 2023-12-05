using AocHelpers;

namespace Aoc2023.Models;

public class RangedMap
{
    private List<Range> Ranges { get; }

    public RangedMap(string init)
    {
        var ranges = init.SplitLines().Where(s => s.Any(c => c.IsDigit()));
        this.Ranges = ranges.Select(s => new Range(s)).OrderBy(r => r.SourceStart).ToList();
    }

    public long Map(long i)
    {
        foreach (var range in this.Ranges)
        {
            if (range.CanMap(i))
            {
                return range.Map(i);
            }
        }

        return i;
    }

    public IEnumerable<(long start, long length)> MapRange(long start, long length)
    {
        long index = start;
        long end = start + length - 1;
        long lengthRemaining = length;
        while (lengthRemaining > 0)
        {
            var rangeCoveringIndex = this.Ranges.SingleOrDefault(r => r.CanMap(index));
            if (rangeCoveringIndex == null) //this segment starts outside a map range
            {
                var nextRange = this.Ranges.FirstOrDefault(r => r.SourceStart >= index && r.SourceStart <= end);
                if (nextRange == null) //there are no more map ranges that cover the target range
                {
                    yield return (index, lengthRemaining); //the rest of the target range is returned unmapped
                    lengthRemaining = 0;
                }
                else //there is a map range that will cover an upcoming segment of the target range
                {
                    var lengthUncovered = nextRange.SourceStart - index;
                    yield return (index, lengthUncovered);
                    lengthRemaining -= lengthUncovered;
                    index += lengthUncovered;
                }
            }
            else //this segment starts inside a map range
            {
                var lengthCovered = Math.Min(rangeCoveringIndex.SourceEnd + 1 - index, lengthRemaining);
                yield return (rangeCoveringIndex.Map(index), lengthCovered);
                lengthRemaining -= lengthCovered;
                index += lengthCovered;
            }
        }
    }

    private class Range
    {
        public Range(string init)
        {
            var parts = init.SplitAndTrim().ToArray();
            this.DestinationStart = long.Parse(parts[0]);
            this.SourceStart = long.Parse(parts[1]);
            this.Length = long.Parse(parts[2]);
        }

        public long DestinationStart { get; }


        public long SourceStart { get; }
        public long SourceEnd => SourceStart + Length - 1;
        public long Length { get; }

        public bool CanMap(long i)
        {
            return i >= this.SourceStart && i <= this.SourceEnd;
        }

        public long Map(long i)
        {
            return i - this.SourceStart + this.DestinationStart;
        }
    }
}