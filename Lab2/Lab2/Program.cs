using System.Net.Security;
using System.Reflection;
using System.Xml;

namespace Lab2
{
    class Program
    {
        private static void Main()
        {
            string sample = "12 +5    / 2-0";
            Console.WriteLine($"Original expression: {sample}");
            
            string newSample = DeleteSpaces(sample);
            Console.WriteLine($"Expression with deleted spaces: {newSample}");   
            
            var stack = new List<char>();
            List<string> numbers = new List<string>();
            List<char> listSigns = new List<char>(){'+', '-', '/', '*'};
            var startIndex = 0;
            for (int i = 1; i < newSample.Length; i++)
            {
                foreach (var elmt in listSigns)
                {
                    if (newSample[i] == elmt)
                    {
                        numbers.Add(newSample.Substring(startIndex, i-startIndex));
                        stack.Add(elmt);
                        startIndex = i + 1;
                    }
                }   
            }
            numbers.Add(newSample.Substring(startIndex, newSample.Length - startIndex));
            var expression = MergeLists(numbers, stack);
            
            Console.WriteLine("Числа в списке:");
            MessageList(numbers);
            Console.WriteLine("Знаки операций:");
            MessageList(stack);
            Console.WriteLine("Выражение:");
            MessageList(expression);
            

            Calculate(stack, numbers);
            var result = numbers[0];
            numbers.Clear();
            Console.WriteLine($"Значение выражения: {result}");
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