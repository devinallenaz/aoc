using System.Collections;

namespace Aoc2022.Models;

public class Move
{
    public int Quantity { get; }
    public int From { get; }
    public int To { get; }
    public Move(int quantity, int from, int to)
    {
        To = to;
        Quantity = quantity;
        From = from;
    }


    public void DoMove(Stack<char>[] stacks)
    {
        for (var i = 0; i < this.Quantity; i++)
        {
            stacks[this.To - 1].Push(stacks[this.From - 1].Pop());
        }
    }

    public void DoMoveUpgraded(Stack<char>[] stacks)
    {
        var crane = new Stack<char>();
        for (var i = 0; i < this.Quantity; i++)
        {
            crane.Push(stacks[this.From - 1].Pop());
        }
        for (var i = 0; i < this.Quantity; i++)
        {
            stacks[this.To - 1].Push(crane.Pop());
        }
    }
}