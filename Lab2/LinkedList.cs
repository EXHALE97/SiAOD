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

        public bool Remove(T item)
        {
            Node<T> previous = null;
            var current = _head;

            while (current != null)
            {
                if (current.Value.Equals(item))
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
                    Count--;
                    return true;
                }

                previous = current;
                current = current.Next;
            }
            return false;
        }

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
            var current = _head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        public bool IsEmpty => Count == 0;
    }
}