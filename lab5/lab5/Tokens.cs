namespace lab5

{
    public abstract class Token
    {
        // Base class for all types of tokens
    }

    public class Parenthesis : Token
    {
        public char Symbol { get; private set; }

        public Parenthesis(char symbol)
        {
            Symbol = symbol;
        }
    }

    public class Number : Token
    {
        public float Value { get; private set; }

        public Number(float value)
        {
            Value = value;
        }
    }

    public class Operation : Token
    {
        public char Symbol { get; }
        public int Priority { get; private set; }

        public Operation(char symbol, int priority)
        {
            Symbol = symbol;
            Priority = priority;
        }

        public static bool operator ==(Operation op1, Operation op2)
        {
            if (ReferenceEquals(op1, null))
                return ReferenceEquals(op2, null);
            
            if (ReferenceEquals(op2, null))
                return false;

            return op1.Symbol == op2.Symbol;
        }

        public static bool operator !=(Operation op1, Operation op2)
        {
            return !(op1 == op2);
        }

        public static bool operator >(Operation op1, Operation op2)
        {
            return op1.Priority > op2.Priority;
        }

        public static bool operator <(Operation op1, Operation op2)
        {
            return op1.Priority < op2.Priority;
        }
        
        public override bool Equals(object? obj)
        {
            if (GetType() != obj.GetType())
            {
                return false;
            }

            Operation other = (Operation)obj;
            return Symbol == other.Symbol;
        }
        
        public static bool operator >=(Operation op1, Operation op2)
        {
            return (op1 > op2) || (op1 == op2);
        }

        public static bool operator <=(Operation op1, Operation op2)
        {
            return (op1 < op2) || (op1 == op2);
        }

        public override int GetHashCode()
        {
            return Symbol.GetHashCode();
        }
    }
}