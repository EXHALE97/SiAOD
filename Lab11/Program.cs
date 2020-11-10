using System;

namespace Lab11
{
    class Program
    {
        static void Main(string[] args)
        {
            var oak = new AvlTree<int> //создание дерева с элементами
            {
                10,
                3,
                2,
                4,
                12,
                15,
                11,
                25
            };

            ShowTree(oak, "Tree before deleting:");

            //                             10                              10                                             
            //                            /   \                           /   \
            //                           /     \                         /     \
            //                          3      12      ====>            3       15
            //                         / \     / \                     / \      / \
            //                        2   4  null 15                  2   4    12  25
            //                                      \              
            //                                       25
            Console.WriteLine("Deleting of element 11...");
            oak.Remove(11); //удаление элемента 11
            ShowTree(oak, "Tree after deleting:"); 
            Console.WriteLine("Finding of element 11:");

            Console.WriteLine(oak.Contains(11) // проверка на наличие элемента 11
                ? "Tree contains current element."
                : "Tree does not contain current element.");

            Console.WriteLine("Finding of element 25:");

            Console.WriteLine(oak.Contains(25) //проверка на наличие элемента 25
                ? "Tree contains current element."
                : "Tree does not contain current element.");

            Console.ReadKey();
        }

        static void ShowTree(AvlTree<int> tree, string title) //вывод дерева с заголовком
        {
            Console.WriteLine(title);
            tree.PrintTree(tree.Head); //вывод дерева начиная от корня
        }
    }
}
