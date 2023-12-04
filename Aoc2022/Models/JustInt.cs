namespace Aoc2022.Models;

public class JustInt : IIntOrIntArray
{
    private int X { get; }

    public JustInt(int x)
    {
        this.X = x;
    }


    public int CompareTo(IIntOrIntArray? other)
    {
        switch (other)
        {
            case JustInt i:
            {
                return this.X - i.X;
            }
            case IntArray a:
            {
                return new IntArray(this).CompareTo(a);
            }
            default:
                throw new NotImplementedException();
        }
    }

    public override string ToString()
    {
        return this.X.ToString();
    }
}