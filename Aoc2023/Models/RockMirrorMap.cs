using System.Text;
using AocHelpers;

namespace Aoc2023.Models;

public class RockMirrorMap
{
    public bool[,] Map { get; }
    public Dictionary<(int, int), bool> ColumnMatchCache { get; } = new Dictionary<(int, int), bool>();
    public Dictionary<(int, int), bool> RowMatchCache { get; } = new Dictionary<(int, int), bool>();

    public RockMirrorMap(string init)
    {
        this.Map = init.To2dBoolArray('.', '#');
    }

    private RockMirrorMap(bool[,] init)
    {
        this.Map = init;
    }

    public string VisualizeMap()
    {
        var stringBuilder = new StringBuilder();
        for (int y = 0; y < this.Map.GetLength(1); y++)
        {
            for (int x = 0; x < this.Map.GetLength(0); x++)
            {
                if (this.Map[x, y])
                {
                    stringBuilder.Append("#");
                }
                else
                {
                    stringBuilder.Append(".");
                }
            }

            stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();
    }

    public IEnumerable<(int, int)> PossibleHorizontalFixes()
    {
        var xPairs = Data.Range(0, this.Map.GetLength(0) - 1).AllPairs();
        foreach (var (x1, x2) in xPairs)
        {
            var possible = PossibleColumnFix(x1, x2);
            if (possible != null)
            {
                yield return possible.Value.Item1;
                yield return possible.Value.Item2;
            }
        }
    }

    public IEnumerable<(int, int)> PossibleVerticalFixes()
    {
        var yPairs = Data.Range(0, this.Map.GetLength(1) - 1).AllPairs();
        foreach (var (y1, y2) in yPairs)
        {
            var possible = PossibleRowFix(y1, y2);
            if (possible != null)
            {
                yield return possible.Value.Item1;
                yield return possible.Value.Item2;
            }
        }
    }


    private ((int, int), (int, int))? PossibleColumnFix(int x1, int x2)
    {
        bool miss = false;
        ((int, int), (int, int))? missIndex = null;
        for (int y = 0; y < this.Map.GetLength(1); y++)
        {
            if (this.Map[x1, y] != this.Map[x2, y])
            {
                if (!miss)
                {
                    miss = true;
                    missIndex = ((x1, y), (x2, y));
                }
                else
                {
                    return null;
                }
            }
        }

        if (miss)
        {
            return missIndex;
        }
        else
        {
            return null;
        }
    }

    private bool ColumnsMatch(int x1, int x2)
    {
        if (ColumnMatchCache.ContainsKey((x1, x2)))
        {
            return ColumnMatchCache[(x1, x2)];
        }

        for (int y = 0; y < this.Map.GetLength(1); y++)
        {
            if (this.Map[x1, y] != this.Map[x2, y])
            {
                ColumnMatchCache[(x1, x2)] = false;
                return false;
            }
        }

        ColumnMatchCache[(x1, x2)] = true;
        return true;
    }

    private ((int, int), (int, int))? PossibleRowFix(int y1, int y2)
    {
        bool miss = false;
        ((int, int), (int, int))? missIndex = null;
        for (int x = 0; x < this.Map.GetLength(0); x++)
        {
            if (this.Map[x, y1] != this.Map[x, y2])
            {
                if (!miss)
                {
                    miss = true;
                    missIndex = ((x, y1), (x, y2));
                }
                else
                {
                    return null;
                }
            }
        }

        if (miss)
        {
            return missIndex;
        }
        else
        {
            return null;
        }
    }

    private bool RowsMatch(int y1, int y2)
    {
        if (RowMatchCache.ContainsKey((y1, y2)))
        {
            return RowMatchCache[(y1, y2)];
        }

        for (int x = 0; x < this.Map.GetLength(0); x++)
        {
            if (this.Map[x, y1] != this.Map[x, y2])
            {
                RowMatchCache[(y1, y2)] = false;
                return false;
            }
        }

        RowMatchCache[(y1, y2)] = true;
        return true;
    }

    public int? FixedVerticalSymmetryIndex()
    {
        var copy = new bool[this.Map.GetLength(0), this.Map.GetLength(1)];
        var originalIndex = this.VerticalSymmetryIndex();
        foreach (var fix in this.PossibleVerticalFixes())
        {
            Array.Copy(this.Map, copy, this.Map.Length);
            copy[fix.Item1, fix.Item2] = !copy[fix.Item1, fix.Item2];
            var vertical = new RockMirrorMap(copy).VerticalSymmetryIndex(originalIndex);
            if (vertical != null)
            {
                return vertical;
            }
        }

        return null;
    }

    public int? VerticalSymmetryIndex(int? ignore=null)
    {
        var index = BottomEdgeSymmetryIndex(ignore);
        if (index != null)
        {
            return index;
        }

        index = TopEdgeSymmetryIndex(ignore);
        if (index != null)
        {
            return index;
        }

        return null;
    }

    private int? TopEdgeSymmetryIndex(int? ignore = null)
    {
        var rowsMatched = 0;
        var mapHeight = this.Map.GetLength(1);
        var resetIndex = 0;

        //search for symmetry that passes the top
        for (int y = mapHeight - 1; y >= 0; y--)
        {
            var otherIndex = 0 + rowsMatched;
            if (otherIndex == y && rowsMatched > 0)
            {
                y = resetIndex;
                rowsMatched = 0;
                continue;
            }

            if (RowsMatch(y, otherIndex))
            {
                if (rowsMatched == 0)
                {
                    resetIndex = y;
                }

                rowsMatched++;


                if (y - 1 == otherIndex)
                {
                    if (y == ignore)
                    {
                        y = resetIndex;
                        rowsMatched = 0;
                    }
                    else
                    {
                        return y;
                    }
                }
            }
            else if (rowsMatched > 0)
            {
                y = resetIndex;
                rowsMatched = 0;
            }
        }

        return null;
    }

    private int? BottomEdgeSymmetryIndex(int? ignore = null)
    {
        var rowsMatched = 0;
        var mapHeight = this.Map.GetLength(1);
        var resetIndex = 0;

        //search for symmetry that passes the bottom
        for (int y = 0; y < mapHeight - 1; y++)
        {
            var otherIndex = mapHeight - rowsMatched - 1;
            if (otherIndex == y && rowsMatched > 0)
            {
                y = resetIndex;
                rowsMatched = 0;
                continue;
            }

            if (RowsMatch(y, otherIndex))
            {
                if (rowsMatched == 0)
                {
                    resetIndex = y;
                }

                rowsMatched++;
                if (y + 1 == otherIndex)
                {
                    if (ignore == otherIndex)
                    {
                        y = resetIndex;
                        rowsMatched = 0;
                    }
                    else
                    {
                        return otherIndex;
                    }
                }
            }
            else if (rowsMatched > 0)
            {
                y = resetIndex;
                rowsMatched = 0;
            }
        }

        return null;
    }

    public int? FixedHorizontalSymmetryIndex()
    {
        var copy = new bool[this.Map.GetLength(0), this.Map.GetLength(1)];
        var originalIndex = this.HorizontalSymmetryIndex();
        foreach (var fix in this.PossibleHorizontalFixes())
        {
            Array.Copy(this.Map, copy, this.Map.Length);
            copy[fix.Item1, fix.Item2] = !copy[fix.Item1, fix.Item2];
            var horizontal = new RockMirrorMap(copy).HorizontalSymmetryIndex(originalIndex);
            if (horizontal != null)
            {
                return horizontal;
            }
        }

        return null;
    }

    public int? HorizontalSymmetryIndex(int? ignore = null)
    {
        var index = RightEdgeSymmetryIndex(ignore);
        if (index != null)
        {
            return index;
        }

        index = LeftEdgeSymmetryIndex(ignore);
        if (index != null)
        {
            return index;
        }

        return null;
    }

    private int? LeftEdgeSymmetryIndex(int? ignore)
    {
        var columnsMatched = 0;
        var mapWidth = this.Map.GetLength(0);
        var resetIndex = 0;

        //search for symmetry that passes the left edge

        for (int x = mapWidth - 1; x >= 0; x--)
        {
            var otherIndex = 0 + columnsMatched;
            if (x == otherIndex && columnsMatched > 0)
            {
                x = resetIndex;
                columnsMatched = 0;
                continue;
            }

            if (ColumnsMatch(x, otherIndex))
            {
                if (columnsMatched == 0)
                {
                    resetIndex = x;
                }

                columnsMatched++;
                if (x - 1 == otherIndex)
                {
                    if (x == ignore)
                    {
                        x = resetIndex;
                        columnsMatched = 0;
                    }
                    else
                    {
                        return x;
                    }
                }
            }
            else
            {
                if (columnsMatched > 0)
                {
                    x = resetIndex;
                    columnsMatched = 0;
                }
            }
        }

        return null;
    }

    private int? RightEdgeSymmetryIndex(int? ignore = null)
    {
        int columnsMatched = 0;
        var mapWidth = this.Map.GetLength(0);
        var resetIndex = 0;
        //search for symmetry that passes the right edge
        for (int x = 0; x < mapWidth - 1; x++)
        {
            var otherIndex = mapWidth - columnsMatched - 1;
            if (x == otherIndex && columnsMatched > 0)
            {
                x = resetIndex;
                columnsMatched = 0;
                continue;
            }

            if (ColumnsMatch(x, otherIndex))
            {
                if (columnsMatched == 0)
                {
                    resetIndex = x;
                }

                columnsMatched++;
                if (x + 1 == otherIndex)
                {
                    if (otherIndex == ignore)
                    {
                        x = resetIndex;
                        columnsMatched = 0;
                    }
                    else
                    {
                        return otherIndex;
                    }
                }
            }
            else
            {
                if (columnsMatched > 0)
                {
                    x = resetIndex;
                    columnsMatched = 0;
                }
            }
        }

        return null;
    }
}