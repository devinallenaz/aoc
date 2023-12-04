using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day4 : Solver
{
    public override int Day => 4;
    public override object ExpectedOutput1 => 13;
    public override object ExpectedOutput2 => 30;

    public override object Solve1(string input)
    {
        var cards = input.SplitLines().Select(l => new ScratchCard(l));
        return cards.Sum(c => c.Score);
    }

    public override object Solve2(string input)
    {
        List<ScratchCard> cards = input.SplitLines().Select(l => new ScratchCard(l)).ToList();
        var stacks = new ScratchCardStack[cards.Count];
        foreach (var card in cards)
        {
            stacks[card.Id - 1] = new ScratchCardStack(card);
        }

        foreach (var stack in stacks)
        {
            for (var j = 0; j < stack.Card.Hits; j++)
            {
                if (stack.Card.Id + j < stacks.Length)
                {
                    stacks[stack.Card.Id + j].Count += stack.Count;
                }
            }
        }

        return stacks.Sum(s=>s.Count);
    }
}