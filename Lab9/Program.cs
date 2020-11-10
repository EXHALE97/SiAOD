using System;
using System.Collections.Generic;

namespace Lab9
{
    class Program
    {
        static void Main(string[] args)
        {
            var initialList = new Dictionary<char, double> //начальные данные
            {
                {'b', 3},
                {'e', 4},
                {'p', 2},
                {' ', 2},
                {'o', 2},
                {'r', 1},
                {'!', 1}
            };
            List<HuffmanNode> nodeList; // список узлов
            Console.Clear();
            nodeList = ProcessMethods.GetNodesFromDictionary(initialList); //получение списка узлов по словарю
            SetColor();
            Console.WriteLine("#Symbols   -   #Frequency");
            SetColorDefault(); //изменение цвета
            ProcessMethods.PrintInformation(nodeList); //вывод информации о символах
            ProcessMethods.GetTreeFromList(nodeList);//создание дерева по символам
            ProcessMethods.SetCodeToTheTree("",nodeList[0]); //установить коды для каждого узла
            Console.WriteLine("\n\n");
            SetColor();
            Console.WriteLine(" #   Huffman Code Tree   # \n");
            SetColorDefault();
            ProcessMethods.PrintTree(0, nodeList[0]); //вывод дерева на экран
            SetColor();
            Console.WriteLine("\n\n#Symbols    -    #Codes\n");
            SetColorDefault();
            ProcessMethods.PrintfLeafAndCodes(nodeList[0]); //вывод символов и их кодов
            Console.WriteLine("\n");
            SetColor();
            Console.Write("\nSymbol with code \'1001\': ");
            SetColorDefault();
            Console.Write(ProcessMethods.GetCharByCode("1001", nodeList[0])); //получение символа по коду
            SetColor();
            Console.Write("\nSymbol with code \'101\': ");
            SetColorDefault();
            Console.Write(ProcessMethods.GetCharByCode("101", nodeList[0]));//получение символа по коду
        }


        //методы, которые меняют цвета в консоли
        public static void SetColor()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public static void SetColorDefault()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
