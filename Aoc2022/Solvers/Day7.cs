using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Solvers;
using File = Aoc2022.Models.File;

namespace Aoc2022.Solvers;

public class Day7 : Solver
{
    public override int Day => 7;
    public override object ExpectedOutput1 => 95437;

    public override object Solve1(string input)
    {
        var folders = FolderStructureFromCommandLine(input);
        return folders.Where(f => f.Size <= 100000).Sum(f => f.Size);
    }

    public override object ExpectedOutput2 => 24933642;

    public override object Solve2(string input)
    {
        var folders = FolderStructureFromCommandLine(input);
        var freeSpace = 70000000 - folders.Max(f => f.Size);
        return folders.Where(f => f.Size + freeSpace > 30000000).Min(f => f.Size);
    }


    private static List<Folder> FolderStructureFromCommandLine(string input)
    {
        var folderCache = new Dictionary<string, Folder>();
        var location = new Stack<string>();
        foreach (var line in input.SplitLines())
        {
            ProcessCommand(line, location, folderCache);
        }

        return folderCache.Values.ToList();
    }

    private static void ProcessCommand(string command, Stack<string> location, Dictionary<string, Folder> allFolders)
    {
        var parts = command.Split();
        var locationName = Path.Combine(location.ToArray().Reverse().ToArray());
        if (parts.Length == 3) //$ cd X
        {
            if (parts[2] == "..")
            {
                location.Pop();
            }
            else
            {
                location.Push(parts[2]);
            }
        }
        else if (parts.Length == 2) //$ ls, dir a, 123123 b.txt
        {
            var currentFolder = allFolders.EnsureFolder(locationName);
            switch (parts[0], parts[1])
            {
                case ("$", "ls"):
                    break;
                case ("dir", var f):
                    var subFolder = allFolders.EnsureFolder(Path.Combine(locationName, f));
                    currentFolder.Contents.Add(subFolder);
                    break;
                case var (s, f):
                    currentFolder.Contents.Add(new File(f, int.Parse(s)));
                    break;
            }
        }
    }


}