using System;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем новую хеш таблицу.
            var hashTable = new HashTable();

            // Добавляем данные в хеш таблицу в виде пар Ключ-Значение.
            hashTable.Insert("Little Prince", "I never wished you any sort of harm; but you wanted me to tame you...");
            hashTable.Insert("Fox", "And now here is my secret, a very simple secret: It is only with the heart that one can see rightly; what is essential is invisible to the eye.");
            hashTable.Insert("Rose", "Well, I must endure the presence of two or three caterpillars if I wish to become acquainted with the butterflies.");
            hashTable.Insert("King", "He did not know how the world is simplified for kings. To them, all men are subjects.");

            // Выводим хранимые значения на экран.
            ShowHashTable(hashTable, "\nCreated hashtable.");
            Console.WriteLine("Press ENTER to continue!");
            Console.ReadLine();

            // Удаляем элемент из хеш таблицы по ключу
            // и выводим измененную таблицу на экран.
            hashTable.Delete("King");
            ShowHashTable(hashTable, "\nDelete item from hashtable.");
            Console.WriteLine("Press ENTER to continue!");
            Console.ReadLine();

            // Получаем хранимое значение из таблицы по ключу.
            Console.WriteLine("\nLittle Prince say:");
            var text = hashTable.Search("Little Prince");
            Console.WriteLine(text);
        }


        // Вывод хеш-таблицы на экран.
        private static void ShowHashTable(HashTable hashTable, string title)
        {
            // Проверяем входные аргументы.
            if (hashTable == null)
            {
                throw new ArgumentNullException(nameof(hashTable));
            }

            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            // Выводим все имеющие пары хеш-значение
            Console.WriteLine(title);
            foreach (var item in hashTable.Items)
            {
                // Выводим хеш
                Console.WriteLine($"Hash: {item.Key}");

                // Выводим все значения хранимые под этим хешем.
                foreach (var value in item.Value)
                {
                    Console.WriteLine($"\t{value.Key} - {value.Value}");
                }
            }
            Console.WriteLine();
        }
    }
}
