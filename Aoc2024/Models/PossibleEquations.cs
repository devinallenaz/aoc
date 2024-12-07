using AocHelpers;

public class PossibleEquation
{
    public PossibleEquation(string input)
    {
        var parts = input.SplitAndTrim(":");
        TestValue = long.Parse(parts.First());
        Operands = parts.Last().SplitAndTrim().Select(long.Parse).Reverse().ToArray();
    }

    public long TestValue { get; }
    public IEnumerable<long> Operands { get; }

    public bool IsPossible => EquationIsPossible(this.TestValue, this.Operands);
    public bool IsPossibleWithConcatenation => EquationIsPossibleWithConcatenation(this.TestValue, this.Operands);

    public static bool EquationIsPossible(long testValue, IEnumerable<long> operands)
    {
        if (!operands.Any())
        {
            return testValue == 0;
        }

        var (currentOperand, remainingOperands) = operands.HeadAndTail();
        return (testValue - currentOperand >= 0 && EquationIsPossible(testValue - currentOperand, remainingOperands))
               || (testValue % currentOperand == 0 && EquationIsPossible(testValue / currentOperand, remainingOperands));
    }

    public static bool EquationIsPossibleWithConcatenation(long testValue, IEnumerable<long> operands)
    {
        if (testValue == -1)
        {
            return false;
        }

        if (!operands.Any())
        {
            return testValue == 0;
        }

        var (currentOperand, remainingOperands) = operands.HeadAndTail();
        return (testValue - currentOperand >= 0 && EquationIsPossibleWithConcatenation(testValue - currentOperand, remainingOperands))
               || (testValue % currentOperand == 0 && EquationIsPossibleWithConcatenation(testValue / currentOperand, remainingOperands))
               || EquationIsPossibleWithConcatenation(Deconcatenate(testValue, currentOperand), remainingOperands);
    }

    public static long Deconcatenate(long complete, long part)
    {
        var completeString = complete.ToString();
        var partString = part.ToString();
        if (!completeString.EndsWith(partString))
        {
            return -1;
        }
        
        return long.Parse(completeString.Substring(0, completeString.Length - partString.Length));
    }
}