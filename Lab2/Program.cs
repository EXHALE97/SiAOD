using System;
using System.Linq;

namespace Lab2
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Insert N:");

            var list = new LinkedList<char>();
            var outputList = new LinkedList<char>();
            var n = Convert.ToInt32(Console.ReadLine());

            AddCharElementsInList(list, n);
            MoveArithmeticSymbolsInSecondList(list, outputList);

            Console.WriteLine("\nFirst list:");
            WriteListInConsole(list);

            if (outputList.IsEmpty)
            {
                Console.WriteLine("\nThere are no arithmetic symbols in the first list.");
            }
            else
            {
                Console.WriteLine("\nSecond list:");
                WriteListInConsole(outputList);
            }
        }

        private static void WriteListInConsole(LinkedList<char> list)
        {
            foreach (var element in list)
            {
                Console.WriteLine(element);
            }
        }

        private static void MoveArithmeticSymbolsInSecondList(LinkedList<char> first, LinkedList<char> second)
        {
            foreach (var element in first)
            {
                if (!IsElementAnArithmeticSymbol(element)) continue;
                second.Add(element);
                first.Remove(element);
            }
        }

        private static void AddCharElementsInList(LinkedList<char> list, int count)
        {
            while (list.Count != count)
            {
                try
                {
                    Console.WriteLine($"\nInsert {count} elements (char only):");

                    for (var i = 0; i < count; i++)
                    {
                        list.Add(Convert.ToChar(Console.ReadLine()));
                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Input error. Try again.");
                    list.Clear();
                }
            }
        }

        private static bool IsElementAnArithmeticSymbol(char element)
        {
            char[] arithmeticSymbols = { '+', '-', '*', '/', '=' };
            return arithmeticSymbols.Contains(element);
        }
    }
}
