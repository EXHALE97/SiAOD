using System;
using System.Collections.Generic;

namespace Lab7
{
    class Program
    {
        static void Main(string[] args)
        {
            var elementCount = 0;
            var expressions = new List<string> //список с выражениями
            {
                "a+b/(c-d)",
                "A+B",
                "?a",
                "a*b*(c-d)",
                "(a^b)+(c/d)",
                "x^y/(5*z)+10"
            };


            foreach (var expression in expressions) //идем по списку и переводим их в префиксную и потфиксную формы
            {
                try
                {
                    Console.WriteLine($"{++elementCount}");
                    Console.WriteLine($"Expression: {expression}");
                    Console.WriteLine($"Postfix form: {Rpn.CalculateToPostfix(expression)}"); //вывод постфиксной формы
                    Console.WriteLine($"Prefix form: {Rpn.CalculateToPrefix(expression)}"); //вывод префиксной формы
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Current expression contains restricted symbols.");
                }
            }
        }
    }
}
