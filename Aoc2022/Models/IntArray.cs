namespace Aoc2022.Models;

public class IntArray : IIntOrIntArray
{
    private IIntOrIntArray[] Children { get; }

    public IntArray(JustInt child)
    {
        Children = new IIntOrIntArray[] { child };
    }

    public IntArray(int child)
    {
        Children = new IIntOrIntArray[] { new JustInt(child) };
    }

    public IntArray(IEnumerable<IIntOrIntArray> children)
    {
        Children = children.ToArray();
    }

    public int CompareTo(IIntOrIntArray? other)
    {
        switch (other)
        {
            case JustInt i:
            {
                return this.CompareTo(new IntArray(i));
            }
            case IntArray a:
            {
                for (var i = 0; i < Math.Min(this.Children.Length, a.Children.Length); i++)
                {
                    var c = this.Children[i].CompareTo(a.Children[i]);
                    if (c != 0)
                    {
                        return c;
                    }
                }

                return this.Children.Length - a.Children.Length;
            }
            default:
                throw new NotImplementedException();
        }
    }

    public override string ToString()
    {
        return $"[{string.Join(",", Children.Select(c => c.ToString()))}]";
    }
}