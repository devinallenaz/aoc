namespace Aoc2022.Models;

public class File : IFileSystemObject
{
    public string Name { get; }
    public int Size { get; }
    
    public File(string name, int size)
    {
        Size = size;
        Name = name;
    }
}