namespace laba
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
            List<char> ListSigns = new List<char>(){'+', '-', '/', '*'};
            var startIndex = 0;
            for (int i = 1; i < newSample.Length; i++)
            {
                foreach (var elmt in ListSigns)
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
            
            MessageList(numbers);
            Console.WriteLine();
            MessageList(stack);
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
        }
    }
}