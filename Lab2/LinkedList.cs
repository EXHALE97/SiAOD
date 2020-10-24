using System.Collections;
using System.Collections.Generic;

namespace Lab2
{
    public class LinkedList<T> : IEnumerable<T>
    {
        private Node<T> _head; //первый элемент
        private Node<T> _tail; //последний элемент
        public int Count { get; private set; }

        public void Add(T item)
        {
            var node = new Node<T>(item); //создаем узел для добавления

            if (_head == null) //если это первый элемент
                _head = node; //то он хед
            else
                _tail.Next = node;
            _tail = node; //если первый - элемент и первый и последний, иначе чисто последнему элементу значение присваиваем

            Count++;
        }

        //удаление элемента
        public bool Remove(T item)
        {
            Node<T> previous = null; //узел который предшествует первому - пустой
            var current = _head; //создаем узел от начала списка, чтобы можно было пройти по всему списку

            // поиск удаляемого узла
            while (current != null) //пока не дойдем до конца списка
            {
                if (current.Value.Equals(item)) //если элемент соответствует тому, который хотим удалить
                {
                    // Если узел в середине или в конце
                    if (previous != null)
                    {
                        // убираем узел current, теперь previous ссылается не на current, а на current.Next
                        previous.Next = current.Next;

                        // Если current.Next не установлен, значит узел последний,
                        // изменяем переменную tail
                        if (current.Next == null)
                            _tail = previous;
                    }
                    else
                    {
                        // если удаляется первый элемент
                        // переустанавливаем значение head
                        _head = _head.Next;

                        // если после удаления список пуст, сбрасываем tail
                        if (_head == null)
                            _tail = null;
                    }
                    Count--; //уменьшаем количество элементов на 1
                    return true;
                }

                //переходим к следующему элементу
                previous = current; //предыдущий элемент будет тем, который выбран сейчас
                current = current.Next; //нынешним элемеентом будет тот, который дальше
            }
            return false;
        }

        //очистка списка
        public void Clear()
        {
            _head = null;
            _tail = null;
            Count = 0;
        }

        //проверка наличия элемента (в условии просили поиск)
        public bool Contains(T item)
        {
            var current = _head;
            while (current != null) //проходим по всем элементам
            {
                if (current.Value.Equals(item)) //если найден - true
                    return true;
                current = current.Next; // или переходим к следующему
            }
            return false;
        }

        //реализуем интерфейс IEnumberable чтобы можно было обращаться к элементам по номерам
        //например list.ElementAt(2). Либо для цикла foreach.
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator(); //просто вызываем базовый метод
        }

        //просто через yield return получаем все элементы списка. Нужно чисто чтобы юзать интерфейс IEnumberable
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            var current = _head; //создаем узел чтобы пройти по всем элементам от начала
            while (current != null) //пока не дошли до конца
            {
                yield return current.Value; //возвращаем каждый имеющийся элемент
                current = current.Next; //идем дальше
            }
        }

        public bool IsEmpty => Count == 0; //если размер равен нулю - список пустой
    }
}