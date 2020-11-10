using System;

namespace Lab15
{
    public class Program
    {
        static void Main()
        {
            var testKeyData = new int[] { 10, 20, 30, 50 };//ключи для добавления
            var testPointerData = new int[] { 50, 60, 40, 20 };//указатели для добавления

            var btree = new BTree<int, int>(2);//создание дерева со степенью 2

            for (var i = 0; i < testKeyData.Length; i++)
            {
                btree.Insert(testKeyData[i], testPointerData[i]); //добавление всех значений из массивов
            }
            
            PrintTree(btree, "Initial tree:");//вывод дерева
            Console.WriteLine("\nDeleting of element 10...");
            btree.Delete(10); //удление элемента 10
            PrintTree(btree, "\nTree after deleting:"); //вывод после удаления
        }

        static void PrintTree(BTree<int, int> tree, string title = null)
        {
            if (title != null)
            {
                Console.WriteLine(title);//вывод заголовка если не пуст
            }

            tree.PrintTree(tree.Root);//вывод дерева на экран наиная от корня
        }
    }
}