using AocHelpers;
using AocHelpers.Solvers;
using Block = (int Id, int Index, int Count);
using EmptySpace = (int index, int count);

public class Day9 : Solver
{
    public override int Day => 9;

    //Problem 1
    public override object ExpectedOutput1 => 1928l;

    private (Stack<Block> occupied, Queue<EmptySpace> empty, int endIndex) Setup(string input)
    {
        var occupied = new Stack<Block>();
        var empty = new Queue<EmptySpace>();

        if (input.Length % 2 != 0)
        {
            input += "0";
        }

        var chars = input.ToCharArray();
        var index = 0;
        for (var i = 0; i < chars.Length / 2; i++)
        {
            var occupiedCount = chars[i * 2].ToNumericInt();
            var emptyCount = chars[(i * 2) + 1].ToNumericInt();
            occupied.Push((i, index, occupiedCount));
            empty.Enqueue((index + occupiedCount, emptyCount));
            index = index + occupiedCount + emptyCount;
        }

        var endIndex = occupied.Sum(b => b.Count);
        return (occupied, empty, endIndex);
    }

    public override object Solve1(string input)
    {
        var (occupied, empty, endIndex) = Setup(input);
        var newBlocks = new List<Block>();
        var currentBlock = occupied.Pop();
        while (empty.TryDequeue(out var currentEmptySpace))
        {
            if (currentEmptySpace.index < endIndex)
            {
                var offset = 0;
                var spaceToFill = currentEmptySpace.count;
                while (spaceToFill != 0)
                {
                    var move = Math.Min(spaceToFill, currentBlock.Count);
                    newBlocks.Add((currentBlock.Id, currentEmptySpace.index + offset, move));
                    offset += move;
                    spaceToFill -= move;
                    currentBlock = (currentBlock.Id, currentBlock.Index, currentBlock.Count - move);
                    if (currentBlock.Count == 0)
                    {
                        currentBlock = occupied.Pop();
                    }
                }
            }
        }

        if (currentBlock.Count > 0)
        {
            occupied.Push(currentBlock);
        }

        var finalBlocks = new List<Block>(occupied).Concat(newBlocks).OrderBy(b => b.Index);
        return finalBlocks.Sum(CalculateBlockChecksum);
    }

    public long CalculateBlockChecksum(Block block)
    {
        var avgIndex = ((decimal)block.Index + (decimal)(block.Index + block.Count - 1)) / 2;
        return (long)(avgIndex * block.Id * block.Count);
    }


    //Problem 2
    public override object ExpectedOutput2 => 2858l;

    public override object Solve2(string input)
    {
        var (occupied, empty, _) = Setup(input);
        var emptyList = empty.ToList();
        var finalBlocks = new List<Block>();
        while (occupied.TryPop(out var currentBlock))
        {
            var space = emptyList.Where(s => s.index < currentBlock.Index && s.count >= currentBlock.Count).DefaultIfEmpty().MinBy(s => s.index);
            if (space == default)
            {
                finalBlocks.Add(currentBlock);
            }
            else
            {
                finalBlocks.Add((currentBlock.Id, space.index, currentBlock.Count));
                emptyList.Remove(space);
                if (space.count > currentBlock.Count)
                {
                    emptyList.Add((space.index + currentBlock.Count, space.count - currentBlock.Count));
                }
            }
        }

        return finalBlocks.Sum(CalculateBlockChecksum);
    }
}