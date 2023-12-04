namespace Aoc2022.Models;

public class Folder : IFileSystemObject
{
    public string Name { get; }
    public List<IFileSystemObject> Contents { get; } = new List<IFileSystemObject>();

    public Folder(string name)
    {
        Name = name;
    }

    public int Size
    {
        get { return this.Contents.Sum(f => f.Size); }
    }
}

public static class FolderHelpers
{
    
    public static Folder EnsureFolder(this Dictionary<string, Folder> folderCache, string name)
    {
        if (!folderCache.ContainsKey(name))
        {
            folderCache.Add(name, new Folder(name));
        }

        return folderCache[name];
    }
}