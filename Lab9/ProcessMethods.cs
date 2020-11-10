using System;
using System.Collections.Generic;

namespace Lab9
{
    class ProcessMethods
    {
        // получаем список узлов по тексту из консоли
        public static List<HuffmanNode> GetNodesFromDictionary(Dictionary<char, double> dictionary)
        {
            List<HuffmanNode> nodeList = new List<HuffmanNode>();  // список узлов
            foreach(var element in dictionary)
            {
                nodeList.Add(new HuffmanNode(element.Key.ToString(), element.Value));
            }
            nodeList.Sort();   // сортируем список по частоте
            return nodeList;

        }


        //  создание дерева по списку
        public static void GetTreeFromList(List<HuffmanNode> nodeList)
        {
            while (nodeList.Count > 1) //пока количество узлов больше 1
            {
                HuffmanNode node1 = nodeList[0];    // первый элемент списка
                nodeList.RemoveAt(0);               // удаляем его
                HuffmanNode node2 = nodeList[0];    // снова получаем первый элемент (второй)
                nodeList.RemoveAt(0);               // удаляем его
                nodeList.Add(new HuffmanNode(node1, node2));    // создаем новый узел на основе этих двух
                nodeList.Sort();     // соритируем список по частоте
            }
        }


        // устанавливаем коды для каждого узла
        public static void SetCodeToTheTree(string code, HuffmanNode nodes)
        {
            if (nodes == null) //если узлов нет - выходим из метода
                return;
            if (nodes.leftTree == null && nodes.rightTree == null) //если нет веток - устаналиваем для входного узла код
            {
                nodes.code = code;
                return;
            }
            SetCodeToTheTree(code + "0", nodes.leftTree);//идем влево - поэтому прибаляем 0 к коду и спускаемся дальше
            SetCodeToTheTree(code + "1", nodes.rightTree);//идем вправо - поэтому прибаляем 1 к коду и спускаемся дальше
        }

        public static string GetCharByCode(string code, HuffmanNode node)
        {
            if (node == null) //если узлов нет - выходим из метода
                return null;

            string result = null;
            if (node.leftTree == null && node.rightTree == null) //если это конечный узел без потомков
            {
                if (node.code.Equals(code)) //если в начальном узле нашли значение
                {
                    result = node.symbol; //присваиваем к результату
                }
                //ничего не нашли - null.
                else return result;
            }
            else
            {
                if (node.code.Equals(code)) //если в начальном узле нашли значение
                {
                    result = node.symbol; //присваиваем к результату
                }
                else
                {
                    result = GetCharByCode(code, node.leftTree); //иначе идем влево

                    if (result == null) //если слева ничего нет
                    {
                        result = GetCharByCode(code, node.rightTree); //идем направо
                    }
                }
            }
            //если ничего так и не нашли - вернется null.
            return result;
        }


        // вывод дерева на экран
        public static void PrintTree(int level, HuffmanNode node)
        {
            if (node == null)//если узел пустой - выходим из метода
                return;
            for (int i = 0; i < level; i++)
            {
                Console.Write("\t");//табуляция
            }
            Console.Write("[" + node.symbol + "]");//вывод символа
            Program.SetColor();
            Console.WriteLine("(" + node.code + ")");//вывод кода
            Program.SetColorDefault();
            PrintTree(level + 1, node.rightTree);//поднимаем уровень дерева и вызываем рекурсивно тот же метод для правой ветки
            PrintTree(level + 1, node.leftTree);//поднимаем уровень дерева и вызываем рекурсивно тот же метод для левой ветки
        }


        //  вывод информации об узлах
        public static void PrintInformation(List<HuffmanNode> nodeList)
        {
            foreach (var item in nodeList)
                Console.WriteLine("Symbol : {0} - Frequency : {1}", item.symbol, item.frequency);//вывод символа и частоты
        }


        // вывод символов и кодов
        public static void PrintfLeafAndCodes(HuffmanNode nodeList) 
        {
            if (nodeList == null)
                return;//если пусто - выходи из метода
            if (nodeList.leftTree == null && nodeList.rightTree == null)//если дальше нет потомков
            {
                Console.WriteLine("Symbol : {0} -  Code : {1}", nodeList.symbol, nodeList.code);//выводим данные и выходим из метода
                return;
            }
            //инаде идем вправо и влево
            PrintfLeafAndCodes(nodeList.leftTree);
            PrintfLeafAndCodes(nodeList.rightTree);
        }

    }
}
