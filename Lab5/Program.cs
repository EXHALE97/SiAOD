using System;
using System.Collections;

namespace Lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nN - 11, M - 10");
            //создание массива из 11 элеменов и значениями, введенными из консоли
            var array = CreateArrayWithElementsFromConsole(11, 24000, 79000); 
            //создание хеш таблицы из чисел, где и ключ и значение - числа.
            var hash = CreateIntHashTableInRange(10, 24000, 79000);

            ShowList(array, "\nInitial array:"); //вывод массива в консоль
            ShowHashTable(hash, "\nInitial hash table:"); //вывод хеш-таблицы в консоль

            Console.WriteLine("\nInsert int value to find for:"); 
            //ввод значения для поиска
            var element = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"\nSearching of value of {element} key in hash table:");
            var searchResult = hash.Search(element.ToString()); //поиск значения по ключу

            if (string.IsNullOrEmpty(searchResult)) //если ничего не найдено
            {
                Console.WriteLine($"There are no any elements with key {element}!"); //ошибка
            }
            else
            {
                Console.WriteLine("\nResult:");
                Console.WriteLine(searchResult); //вывод результата
            }

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

        //метод вывода списка в консоль
        private static void ShowList(IEnumerable list, string title = null)
        {
            if (!string.IsNullOrEmpty(title)) //если заголовок не пустой
            {
                Console.WriteLine(title); //выводим его в консоль
            }

            foreach (var element in list) //перебираем каждый элемент
            {
                Console.WriteLine(element); //и выводим в консоль
            }
        }

        //создание массива из элементов из консоли
        static ArrayList CreateArrayWithElementsFromConsole(int count, int minValue, int maxValue)
        {
            var array = new ArrayList();

            while (array.Count != count) //пока количество элементов не равно count
            {
                try
                {
                    Console.WriteLine($"Insert {count} elements (int only):");

                    for (var i = 0; i < count; i++) //вызываем N раз ввод данных из консоли
                    {
                        var element = Convert.ToInt32(Console.ReadLine());
                        if (!IsIntElementInTheRange(element, minValue, maxValue)) //если элемент за граниными значениями
                        {
                            Console.WriteLine("Item is not in range. Try again."); //просим ввести символы заново
                            array.Clear();
                            break;
                        }
                        array.Add(element); //добавляем введенное значение в список
                    }
                }
                catch (FormatException) //если вдруг введенный элемент не является char (символом)
                {
                    Console.WriteLine("Input error. Try again."); //просим ввести символы заново
                    array.Clear(); //очищаем список
                }
            }
            
            return array;
        }

        static HashTable CreateIntHashTableInRange(int count, int minValue, int maxValue)
        {
            var hashTable = new HashTable();

            for (var i = 0; i < count; i++) //вызываем N раз ввод данных из консоли
            {
                var number = RandomNumber(minValue, maxValue).ToString(); //занчение - рандомное чисто в диапазоне
                hashTable.Insert(number, number); //добавляем введенное значение в список
            }

            return hashTable;
        }

        //находится ли элемент в дапазоне
        static bool IsIntElementInTheRange(int element, int minValue, int MaxValue)
        {
            return element >= minValue && element <= MaxValue; //если находится - true, еслии нет - false
        }

        //возвращает рандомное чисто в диапазоне
        static int RandomNumber(int min, int max)
        { 
            return new Random().Next(min, max); //возвращает рандомное чисто в диапазоне
        }
    }
}
