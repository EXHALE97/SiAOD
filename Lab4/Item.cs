using System;

namespace Lab4
{
    /// Элемент данных хеш таблицы.
    public class Item
    {
        /// Ключ.
        public string Key { get; private set; }

        /// Хранимые данные.
        public string Value { get; private set; }

        /// Создание экземпляра хранимых данных Item.
        public Item(string key, string value)
        {
            // Проверяем входные данные на корректность.
            if (string.IsNullOrEmpty(key)) //если ключ пустой
            {
                throw new ArgumentNullException(nameof(key)); //ошибка
            }

            if (string.IsNullOrEmpty(value)) //если значение пустое
            {
                throw new ArgumentNullException(nameof(value));//ошибка
            }

            // Устанавливаем значения.
            Key = key;
            Value = value;
        }


        /// Приведение объекта к строке.
        public override string ToString()
        {
            return Key;
        }
    }
}