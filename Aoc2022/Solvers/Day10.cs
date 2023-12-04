using System.Text;
using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day10 : Solver
{
    public override int Day => 10;
    public override object ExpectedOutput1 => 13140;

    public override object Solve1(string input)
    {
        var cpu = CpuFromData(input);
        var sum = 0;
        for (var i = 1; i <= 220; i++)
        {
            var signal = cpu.Cycle() * i;
            if ((i + 20) % 40 == 0)
            {
                sum += signal;
            }
        }

        return sum;
    }

    public override object ExpectedOutput2 => @"
##..##..##..##..##..##..##..##..##..##..
###...###...###...###...###...###...###.
####....####....####....####....####....
#####.....#####.....#####.....#####.....
######......######......######......####
#######.......#######.......#######.....";

    public override object Solve2(string input)
    {
        var cpu = CpuFromData(input);
        var output = new StringBuilder();
        for (var i = 1; i <= 240; i++)
        {
            if (Math.Abs(cpu.Cycle() - (i - 1) % 40) <= 1)
            {
                output.Append('#');
            }
            else
            {
                output.Append('.');
            }

            if (i % 40 == 0)
            {
                output.Append('\n');
            }
        }

        return "\n" + output.ToString().Trim();
    }
    
    public static Cpu CpuFromData(string input)
    {
        return new Cpu(input.SplitLines());
    }

}