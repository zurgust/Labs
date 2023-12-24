using static System.Console;
using static lab4.Meths;

namespace lab4
{
    static class Program
    {
        static void Main()
        {
            Write("Your expression: ");
            string inputExpr = "55+89 *(3-1)-99/33 ";
            Write(inputExpr);
            inputExpr = DeleteSpaces(inputExpr);
            List<object> parsedExpr = Parse(inputExpr);

            List<object> rpn = ToReversePolishNotation(parsedExpr);
            WriteLine($"\nReverse Polish notation: {string.Join(" ", rpn)}");
            float result = Calculate(rpn);

            WriteLine($"Result: {result}");
            ReadLine();
        }
    }
}