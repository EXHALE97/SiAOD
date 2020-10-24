using System;
using System.Linq;

namespace Lab2
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Insert N:");

            var list = new LinkedList<char>(); //первый список
            var outputList = new LinkedList<char>(); //второй список
            var n = Convert.ToInt32(Console.ReadLine()); // n, введенное с клавиатуры

            AddCharElementsInList(list, n); //вызов функции добавления символов в список
            MoveArithmeticSymbolsInSecondList(list, outputList); //перемещение арифметических символов в другой список

            Console.WriteLine("\nFirst list:");
            WriteListInConsole(list); //вывод первого списка в консоль

            if (outputList.IsEmpty) //если во втором списке пусто (в первом не было арифметических символов)
            {
                Console.WriteLine("\nThere are no arithmetic symbols in the first list.");
            }
            else
            {
                Console.WriteLine("\nSecond list:");
                WriteListInConsole(outputList); //вывод второго списка в консоль
            }
        }

        //метод вывода списка в консоль
        private static void WriteListInConsole(LinkedList<char> list)
        {
            foreach (var element in list) //перебираем каждый элемент
            {
                Console.WriteLine(element); //и выводим в консоль
            }
        }

        //метод перемещения арифметических символов в другой список
        private static void MoveArithmeticSymbolsInSecondList(LinkedList<char> first, LinkedList<char> second)
        {
            foreach (var element in first) //перебираем элементы
            {
                if (!IsElementAnArithmeticSymbol(element)) continue; //если элемент не арифметиеский символ - переходим дальше
                second.Add(element); //иначе добавляем элемент во второй список
                first.Remove(element); //и удаляем из первого
            }
        }

        //метод добавления символов в список (введеных в консоль)
        private static void AddCharElementsInList(LinkedList<char> list, int count)
        {
            while (list.Count != count) //пока список не достигнет размера N
            {
                try
                {
                    Console.WriteLine($"\nInsert {count} elements (char only):");

                    for (var i = 0; i < count; i++) //вызываем N раз ввод данных из консоли
                    {
                        list.Add(Convert.ToChar(Console.ReadLine())); //добавляем введенное значение в список
                    }
                }
                catch (FormatException e) //если вдруг введенный элемент не является char (символом)
                {
                    Console.WriteLine("Input error. Try again."); //просим ввести символы заново
                    list.Clear(); //очищаем список
                }
            }
        }

        //проверка на то, является ли список арифметическим символом
        private static bool IsElementAnArithmeticSymbol(char element)
        {
            char[] arithmeticSymbols = { '+', '-', '*', '/', '=' };
            return arithmeticSymbols.Contains(element); //если элемент входит в массив выше - true, иначе false
        }
    }
}
