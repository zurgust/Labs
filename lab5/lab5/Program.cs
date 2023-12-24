using static System.Console;
using static lab5.Meths;

namespace lab5
{
    static class Program
    {
        static void Main()
        {
            Write("Your expression: ");
            string inputExpr = ReadLine();
            inputExpr = DeleteSpaces(inputExpr);
            List<Token> parsedExpr = Parse(inputExpr);

            List<Token> rpn = ToReversePolishNotation(parsedExpr);
            WriteLine("\nReverse Polish Notation:");

            foreach (var token in rpn)
            {
                if (token is Number num)
                {
                    Write(num.Value + " ");
                }
                else if (token is Operation op)
                {
                    Write(op.Symbol + " ");
                }
            }
            WriteLine();
        }
    }
}