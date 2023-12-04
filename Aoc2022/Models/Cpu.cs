namespace Aoc2022.Models;

public class Cpu
{
    public int Register { get; private set; }

    private int? CurrentInstruction { get; set; }
    private IEnumerator<string> InstructionEnumerator { get; }

    public Cpu(IEnumerable<string> instructions)
    {
        this.Register = 1;
        this.InstructionEnumerator = instructions.GetEnumerator();
    }

    public int Cycle()
    {
        var retVal = this.Register;
        if (CurrentInstruction != null)
        {
            this.Register += CurrentInstruction.Value;
            this.CurrentInstruction = null;
        }
        else
        {
            InstructionEnumerator.MoveNext();
            var instruction = InstructionEnumerator.Current;
            var parts = instruction.Split();
            switch (parts.First(), parts.Last())
            {
                case ("noop", _):
                    break;
                case ("addx", var i):
                    CurrentInstruction = int.Parse(i);
                    break;
            }

        }

        return retVal;
    }
}