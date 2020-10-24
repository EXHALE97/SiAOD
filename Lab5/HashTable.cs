using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab5
{
    // Хеш-таблица.
    // Используется метод цепочек (открытое хеширование).
    public class HashTable
    {
        // Максимальная длина ключевого поля.
        private const byte MaxSize = 255;


        // Коллекция хранимых данных.
        // Представляет собой словарь, ключ которого представляет собой хеш ключа хранимых данных,
        // а значение это список элементов с одинаковым хешем ключа.
        private Dictionary<int, List<Item>> _items = null;


        // Коллекция хранимых данных в хеш-таблице в виде пар Хеш-Значения.
        public IReadOnlyCollection<KeyValuePair<int, List<Item>>> Items => _items?.ToList()?.AsReadOnly();


        // Создать новый экземпляр класса HashTable.
        public HashTable()
        {
            // Инициализируем коллекцию максимальным количество элементов.
            _items = new Dictionary<int, List<Item>>(MaxSize);
        }


        // Добавить данные в хеш таблицу.
        public void Insert(string key, string value)
        {
            // Проверяем входные данные на корректность.
            if (string.IsNullOrEmpty(key)) //если ключ пустой
            {
                throw new ArgumentNullException(nameof(key)); //ошибка
            }

            if (key.Length > MaxSize) //если длина ключа больше максимальной
            {
                throw new ArgumentException($"Максимальная длинна ключа составляет {MaxSize} символов.", nameof(key)); //ошибка
            }

            if (string.IsNullOrEmpty(value)) //если значение пустое
            {
                throw new ArgumentNullException(nameof(value)); //ошибка
            }

            // Создаем новый экземпляр данных.
            var item = new Item(key, value);

            // Получаем хеш ключа
            var hash = GetHash(item.Key);

            // Получаем коллекцию элементов с таким же хешем ключа.
            // Если коллекция не пустая, значит заначения с таким хешем уже существуют,
            // следовательно добавляем элемент в существующую коллекцию.
            // Иначе коллекция пустая, значит значений с таким хешем ключа ранее не было,
            // следовательно создаем новую пустую коллекцию и добавляем данные.
            List<Item> hashTableItem = null;
            if (_items.ContainsKey(hash)) //если такой ключ уже есть
            {
                // Получаем элемент хеш таблицы.
                hashTableItem = _items[hash];

                // Проверяем наличие внутри коллекции значения с полученным ключом.
                // Если такой элемент найден, то сообщаем об ошибке.
                var oldElementWithKey = hashTableItem.SingleOrDefault(i => i.Key == item.Key);
                if (oldElementWithKey != null)
                {
                    throw new ArgumentException($"Хеш-таблица уже содержит элемент с ключом {key}. Ключ должен быть уникален.", nameof(key));
                }

                // Добавляем элемент данных в коллекцию элементов хеш таблицы.
                _items[hash].Add(item);
            }
            else
            {
                // Создаем новую коллекцию.
                hashTableItem = new List<Item> { item };

                // Добавляем данные в таблицу.
                _items.Add(hash, hashTableItem);
            }
        }


        // Удалить данные из хеш таблицы по ключу.
        public void Delete(string key)
        {
            // Проверяем входные данные на корректность.
            if (string.IsNullOrEmpty(key)) //если ключ пустой
            {
                throw new ArgumentNullException(nameof(key)); //ошибка
            }

            if (key.Length > MaxSize) //если длина ключа больше максимальной
            {
                throw new ArgumentException($"Максимальная длинна ключа составляет {MaxSize} символов.", nameof(key)); //ошибка
            }

            // Получаем хеш ключа.
            var hash = GetHash(key);

            // Если значения с таким хешем нет в таблице, 
            // то завершаем выполнение метода.
            if (!_items.ContainsKey(hash))
            {
                return;
            }

            // Получаем коллекцию элементов по хешу ключа.
            var hashTableItem = _items[hash];

            // Получаем элемент коллекции по ключу.
            var item = hashTableItem.SingleOrDefault(i => i.Key == key);

            // Если элемент коллекции найден, 
            // то удаляем его из коллекции.
            if (item != null)
            {
                hashTableItem.Remove(item);
            }
        }


        // Поиск значения по ключу.
        // Найденные по ключу хранимые данные.
        public string Search(string key)
        {
            // Проверяем входные данные на корректность.
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.Length > MaxSize)
            {
                throw new ArgumentException($"Максимальная длинна ключа составляет {MaxSize} символов.", nameof(key));
            }

            // Получаем хеш ключа.
            var hash = GetHash(key);

            // Если таблица не содержит такого хеша,
            // то завершаем выполнения метода вернув null.
            if (!_items.ContainsKey(hash))
            {
                return null;
            }

            // Если хеш найден, то ищем значение в коллекции по ключу.
            var hashTableItem = _items[hash];

            // Если хеш найден, то ищем значение в коллекции по ключу.
            if (hashTableItem != null)
            {
                // Получаем элемент коллекции по ключу.
                var item = hashTableItem.SingleOrDefault(i => i.Key == key);

                // Если элемент коллекции найден, 
                // то возвращаем хранимые данные.
                if (item != null)
                {
                    return item.Value;
                }
            }

            // Возвращаем null если ничего найдено.
            return null;
        }


        // Хеш функция.
        // Возвращает длину строки.
        private int GetHash(string value)
        {
            // Проверяем входные данные на корректность.
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Length > MaxSize)
            {
                throw new ArgumentException($"Максимальная длинна ключа составляет {MaxSize} символов.", nameof(value));
            }

            // Получаем длину строки.
            var hash = value.Length;
            return hash;
        }
    }
}