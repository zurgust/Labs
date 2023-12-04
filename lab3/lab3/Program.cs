namespace lab3
{
    class Program
    {
        static readonly List<char> ListSigns = new List<char>(){'+', '-', '/', '*'};
        private static void Main()
        {
            string sample = "((12 +5)    / 2)-0";
            Console.WriteLine($"Original expression: {sample}");
            
            string newSample = DeleteSpaces(sample);
            Console.WriteLine($"Expression with deleted spaces: {newSample}");   
            
            var (originalNumbers, originalStack) = CreateLocalStackAndNumbersList(newSample);
            var expression = MergeLists(originalNumbers, originalStack);
            
            Console.WriteLine("Числа со скобками в списке:");
            MessageList(originalNumbers);
            Console.WriteLine("Знаки операций:");
            MessageList(originalStack);
            Console.WriteLine("Выражение:");
            MessageList(expression);
            
            if (newSample.Contains('('))
            {
                bool isBracketExist = true;

                do
                {
                    int indexFirstBracket = newSample.LastIndexOf('(');
                    int indexLastBracket = newSample.IndexOf(')');
                    int countBetweenBrackets = indexLastBracket - indexFirstBracket - 1;
                    string bracketSample = newSample.Substring(indexFirstBracket + 1, countBetweenBrackets);
                    var num = SolveBrackets(bracketSample);

                    newSample = newSample.Replace($"({bracketSample})", num);
                    isBracketExist = newSample.Contains('(');
                } while (isBracketExist);
            }
            
            var (numbers, stack) = CreateLocalStackAndNumbersList(newSample);

            
            Calculate(stack, numbers);
            var result = numbers[0];
            numbers.Clear();
            Console.WriteLine($"Значение выражения: {result}");
        }

        private static string SolveBrackets(string bracketSample)
        {
            string result;
            var (localNumbers,localStack) = CreateLocalStackAndNumbersList(bracketSample);
            Calculate(localStack, localNumbers);
            result = localNumbers[0];
            return result;
        }

        private static (List<string> localNumbers, List<char> localStack) CreateLocalStackAndNumbersList(string localSample)
        {
            List<string> localNumbers = new List<string>();
            List<char> localStack = new List<char>();
            
            var startIndex = 0;
            for (int i = 1; i < localSample.Length; i++)
            {
                foreach (var elmt in ListSigns)
                {
                    if (localSample[i] == elmt)
                    {
                        localNumbers.Add(localSample.Substring(startIndex, i - startIndex));
                        localStack.Add(elmt);
                        startIndex = i + 1;
                    }
                }
            }
            localNumbers.Add(localSample.Substring(startIndex, localSample.Length - startIndex));
            return (localNumbers: localNumbers, localStack: localStack);
        }

        private static void Calculate(List<char> stack, List<string> numbers)
        {
            DevideAndMultiply(stack, numbers);
            AddAndSubtract(stack, numbers);
        }
        
        private static void DevideAndMultiply(List<char> stack, List<string> numbers)
        {
            while (stack.Contains('/') || stack.Contains('*'))
            {
                var (index, isMultiplication) = FindFirstDivideOrMultiply(stack);
                if (index == stack.Count) return; 
                
                double result = !isMultiplication ? Divide(stack, numbers, index) : Multiply(stack, numbers, index);
                
                numbers.Insert(index, $"{result}");
            }
        }

        private static (int index, bool isMultiplication) FindFirstDivideOrMultiply(List<char> stack)
        {
            bool isMultiplication = false;
            int index = stack.Count;
            
            int multiplyId = stack.IndexOf('*');
            if (multiplyId >= 0)
            {
                index = multiplyId;
                isMultiplication = true;
            }
            
            int divideId = stack.IndexOf('/');
            if (divideId >= 0 && divideId < index)
            {
                index = divideId;
                isMultiplication = false;
            }
            
            return (index: index, isMultiplication: isMultiplication);
        }

        private static void AddAndSubtract(List<char> stack, List<string> numbers)
        {
            while (stack.Contains('+') || stack.Contains('-'))
            {
                var (index, isAdd) = FindFirstAddOrSubtract(stack);
                if (index == stack.Count) return; 
                
                double result = !isAdd ? Subtract(stack, numbers, index) : Add(stack, numbers, index);
                
                numbers.Insert(index, $"{result}");
            }
        }

        private static (int index, bool isAdd) FindFirstAddOrSubtract(List<char> stack)
        {
            bool isAdd = false;
            int index = stack.Count;

            int addId = stack.IndexOf('+');
            if (addId >= 0)
            {
                index = addId;
                isAdd = true;
            }
            
            int subtractId = stack.IndexOf('-');
            if (subtractId >= 0 && subtractId < index)
            {
                index = subtractId;
                isAdd = false;
            }
            
            return (index: index, isAdd: isAdd);
        }

        public static double Divide(List<char> stack, List<string> numbers, int id)
        {
            double a = double.Parse(numbers[id]);
            double b = double.Parse(numbers[id + 1]);
            stack.Remove('/');
            numbers.RemoveRange(id, 2);
            return a / b;
        }
        
        public static double Multiply(List<char> stack, List<string> numbers, int id)
        {
            double a = double.Parse(numbers[id]);
            double b = double.Parse(numbers[id + 1]);
            stack.Remove('*');
            numbers.RemoveRange(id, 2);
            return a * b;
        }
        public static double Add(List<char> stack, List<string> numbers, int id)
        {
            double a = double.Parse(numbers[id]);
            double b = double.Parse(numbers[id + 1]);
            stack.Remove('+');
            numbers.RemoveRange(id, 2);
            return a + b;
        }
        public static double Subtract(List<char> stack, List<string> numbers, int id)
        {
            double a = double.Parse(numbers[id]);
            double b = double.Parse(numbers[id + 1]);
            stack.Remove('-');
            numbers.RemoveRange(id, 2);
            return a - b;
        }
        
        public static List<string> MergeLists(List<string> numbers, List<char> operations)
        {
            List<string> expression = new List<string>();
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                expression.Add(numbers[i]);
                expression.Add(Convert.ToString(operations[i]));
            }
            int k = numbers.Count - 1;
            expression.Add(numbers[k]);
            return expression;
        }
        
        public static string DeleteSpaces(string str)
        {
            return str.Replace(" ", String.Empty);
        }

        public static void MessageList<T>(List<T> anyList)
        {
            foreach (var element in anyList)
            {
                Console.Write($"{element}\t");
            }
            Console.WriteLine();
        }
    }
}