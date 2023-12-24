using System.Globalization;

namespace lab5;

public static class Meths
{
    public static string DeleteSpaces(string str)
    {
        return str.Replace(" ", String.Empty);
    }
    static float GetNumber(object operation, float firstOperator, float secondOperator)
    {
        switch (operation)
        {
            case '+': 
                return firstOperator + secondOperator;
            case '-': 
                return firstOperator - secondOperator;
            case '*': 
                return firstOperator * secondOperator;
            case '/': 
                return firstOperator / secondOperator;
            default: 
                return 0;
        }
    }

    public static List<Token> Parse(string expression)
    {
        List<Token> tokens = new List<Token>();
        for (int i = 0; i < expression.Length; i++)
        {
            if (char.IsDigit(expression[i]) || (expression[i] == '.'))
            {
                var start = i;
                while (i + 1 < expression.Length && (char.IsDigit(expression[i + 1]) || (expression[i + 1] == '.')))
                {
                    i++;
                }
                tokens.Add(new Number(float.Parse(expression.Substring(start, i - start + 1), CultureInfo.InvariantCulture)));
            }
            else if (expression[i] == '+' || expression[i] == '-' || expression[i] == '*' || expression[i] == '/')
            {
                int priority = (expression[i] == '+' || expression[i] == '-') ? 1 : 2;
                tokens.Add(new Operation(expression[i], priority));
            }
            else if (expression[i] == '(' || expression[i] == ')')
            {
                tokens.Add(new Parenthesis(expression[i]));
            }
        }
        return tokens;
    }

    public static List<Token> ToReversePolishNotation(List<Token> tokens)
    {
        List<Token> output = new List<Token>();
        Stack<Token> operators = new Stack<Token>();

        foreach (Token token in tokens)
        {
            switch (token)
            {
                case Number number:
                    output.Add(number);
                    break;
                case Operation operation:
                    while (operators.Count > 0 && operators.Peek() is Operation peekedOp && peekedOp >= operation)
                    {
                        output.Add(operators.Pop());
                    }
                    operators.Push(operation);
                    break;
                case Parenthesis parenthesis:
                    if (parenthesis.Symbol == '(')
                    {
                        operators.Push(parenthesis);
                    }
                    else // если встретилась закрывающая скобка
                    {
                        // Извлекаем операторы до открывающей скобки
                        while (operators.Count > 0 && !(operators.Peek() is Parenthesis op && op.Symbol == '('))
                        {
                            output.Add(operators.Pop());
                        }
                        if (operators.Count > 0 && operators.Peek() is Parenthesis)
                        {
                            operators.Pop(); // Извлекаем открывающую скобку
                        }
                        else
                        {
                            throw new InvalidOperationException("No matching opening parenthesis found.");
                        }
                    }
                    break;

            }
        }

        while (operators.Count > 0)
        {
            output.Add(operators.Pop()); // Pop any remaining operators
        }

        return output;
    }

    public static float Calculate(List<object> rpn)
    {
        for (int i = 0; i < rpn.Count; i++)
        {
            if (rpn[i] is char)
            {
                float firstNumber = Convert.ToSingle(rpn[i - 2]);
                float secondNumber = Convert.ToSingle(rpn[i - 1]);
                float result = GetNumber(rpn[i], firstNumber, secondNumber);

                rpn.RemoveRange(i - 2, 3);
                rpn.Insert(i - 2, result);
                i -= 2;
            }
        }
        return (float)rpn[0];
    }
}