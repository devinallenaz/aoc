namespace AocHelpers.Models;

public class RingItem<T>
{
    public RingItem(int originalIndex, T value, RingItem<T>? previous)
    {
        this.OriginalIndex = originalIndex;
        if (previous != null)
        {
            this._previous = previous;
            previous._next = this;
        }

        this.Value = value;
        OriginalIndex = originalIndex;
    }

    public void SetPrevious(RingItem<T> previous)
    {
        this._previous = previous;
        previous._next = this;
    }

    public void SetNext(RingItem<T> next)
    {
        this._next = next;
        next._previous = this;
    }

    public int OriginalIndex { get; }
    private RingItem<T>? _previous = null;
    public RingItem<T> Previous => _previous ?? throw new ApplicationException("Uninitialized ring");
    private RingItem<T>? _next = null;
    public RingItem<T> Next => _next ?? throw new ApplicationException("Uninitialized ring");
    public T Value { get; }
    

}