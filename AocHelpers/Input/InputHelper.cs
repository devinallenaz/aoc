using System.Reflection;

namespace AocHelpers.Input;

public static class InputHelper
{
    public static string GetInputText(int day, bool test = false, bool problem2 = false)
    {
        if (test && problem2 && File.Exists($"/Users/devinallen/projects/Aoc/Aoc{Runner.Year}/Input/day{day}_test_2.txt"))
        {
            return File.ReadAllText($"/Users/devinallen/projects/Aoc/Aoc{Runner.Year}/Input/day{day}_test_2.txt");
        }
        return File.ReadAllText($"/Users/devinallen/projects/Aoc/Aoc{Runner.Year}/Input/day{day}{(test ? "_test" : "")}.txt");
    }
}