using System.Text.RegularExpressions;
using AocHelpers;

namespace Aoc2024.Models;

public class Computer
{
    public Computer(long registerA)
    {
        RegisterA = registerA;
    }

    public string RunProgram(string input)
    {
        var program = input.SplitCommas().Select(int.Parse).ToArray();

        while (InstructionPointer < program.Length)
        {
            Instruction(program[InstructionPointer])(program[InstructionPointer + 1]);
        }

        return string.Join(",", Output);
    }

    public List<int> RunProgramDigits(string input)
    {
        var program = input.SplitCommas().Select(int.Parse).ToArray();

        while (InstructionPointer < program.Length)
        {
            Instruction(program[InstructionPointer])(program[InstructionPointer + 1]);
        }

        return Output;
    }

    private long RegisterA { get; set; }
    private long RegisterB { get; set; }
    private long RegisterC { get; set; }
    private int InstructionPointer { get; set; } = 0;
    public List<int> Output = [];

    private Action<int> Instruction(int opcode)
    {
        return opcode switch
        {
            0 => Adv,
            1 => Bxl,
            2 => Bst,
            3 => Jnz,
            4 => Bxc,
            5 => Out,
            6 => Bdv,
            7 => Cdv,
            _ => throw new NotImplementedException(),
        };
    }

    private long Combo(int operand)
    {
        return operand switch
        {
            < 4 => operand,
            4 => RegisterA,
            5 => RegisterB,
            6 => RegisterC,
            > 6 => throw new NotImplementedException(),
        };
    }

    private void Adv(int operand)
    {
        RegisterA = (long)(RegisterA / Math.Pow(2, Combo(operand)));
        InstructionPointer += 2;
    }

    private void Bxl(int operand)
    {
        RegisterB ^= operand;
        InstructionPointer += 2;
    }

    private void Bst(int operand)
    {
        RegisterB = Combo(operand) % 8;
        InstructionPointer += 2;
    }

    private void Jnz(int operand)
    {
        if (RegisterA != 0)
        {
            InstructionPointer = operand;
        }
        else
        {
            InstructionPointer += 2;
        }
    }

    private void Bxc(int operand)
    {
        RegisterB ^= RegisterC;
        InstructionPointer += 2;
    }

    private void Out(int operand)
    {
        Output.Add((int)(Combo(operand) % 8));
        InstructionPointer += 2;
    }

    private void Bdv(int operand)
    {
        RegisterB = (long)(RegisterA / Math.Pow(2, Combo(operand)));
        InstructionPointer += 2;
    }

    private void Cdv(int operand)
    {
        RegisterC = (long)(RegisterA / Math.Pow(2, Combo(operand)));
        InstructionPointer += 2;
    }
}