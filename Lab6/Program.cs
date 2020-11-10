using System;

namespace Lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            var queue = new Queue<string>(); //создание очереди
            queue.Enqueue("Kate"); //добавлем элементы
            queue.Enqueue("Sam");
            queue.Enqueue("Alice");
            queue.Enqueue("Tom");

            ShowQueue(queue, "Initial queue:\n"); //вывод исходной очереди на экран
            Console.WriteLine($"\nDeleted element: {queue.Dequeue()}\n"); //удаляем элемент и выводим его значение в консоль
            ShowQueue(queue, "Queue after deleting:\n"); //вывод очереди после удаление

            Console.WriteLine("\nSearching for element \"Kate\":");
            Console.WriteLine(queue.Contains("Kate") //проверяем очередь на наличие элемента Kate
                ? "Queue contains element \"Kate\":\n"
                : "Queue does not contain element \"Kate\":\n");

            Console.WriteLine("\nSearching for element \"Sam\":");
            Console.WriteLine(queue.Contains("Sam") //проверяем очередь на наличие элемента Sam
                ? "Queue contains element \"Sam\":\n"
                : "Queue does not contain element \"Sam\":\n");
        }

        //вывод очереди в консоль
        static void ShowQueue(Queue<string> queue, string title = null)
        {
            if (title != null)
            {
                Console.WriteLine(title); //вывод в консоль заголовка если он задан
            } 
            foreach (var item in queue) //вывод очереди поэлементно в консоль
                Console.WriteLine(item);
        }
    }
}
