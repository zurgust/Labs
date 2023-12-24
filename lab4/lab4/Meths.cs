namespace lab4;

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
    
    static int GetPriority(object operation) 
    {
        switch (operation)
        {
            case '-': 
                return 1;
            case '+': 
                return 1;
            case '*':
                return 2;
            case '/': 
                return 2;
            default: 
                return 0;
        }
    }

    public static List<object> Parse(string? expression)
    {
        List<object> result = new List<object>();

        string number = "";

        foreach (char token in expression!)
        {
            if (!char.IsDigit(token))
            {
                if (number != "") result.Add(number);
                result.Add(token);
                number = "";
            }
            else
            {
                number += token;
            }
        }

        if (number != "")
        {
            result.Add(number);
        }

        return result;
    }

    public static List<object> ToReversePolishNotation(List<object> expression)
    {
        Stack<object> operations = new Stack<object>();

        List<object> result = new List<object>();

        foreach (object token in expression)
        {
            if (token is string)
            {
                result.Add(token);
            }
            else if (token.Equals('+') || token.Equals('-') || token.Equals('*') || token.Equals('/'))
            {
                while (operations.Count > 0 && GetPriority(operations.Peek()) >= GetPriority(token))
                {
                    result.Add(operations.Pop());
                }
                operations.Push(token);
            }
            else if (token.Equals('('))
            {
                operations.Push(token);
            }
            else if (token.Equals(')'))
            {
                while (operations.Count > 0 && !operations.Peek().Equals('('))
                {
                    result.Add(operations.Pop());
                }
                operations.Pop();
            }
        }

        while (operations.Count > 0)
        {
            result.Add(operations.Pop());
        }

        return result;
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