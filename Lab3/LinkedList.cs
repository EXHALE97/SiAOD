using System.Collections;
using System.Collections.Generic;

namespace Lab3
{
    public class LinkedList<T> : IEnumerable<T>
    {
        private Node<T> _head; //первый элемент
        private Node<T> _tail; //последний элемент
        public int Count { get; private set; }

        //добавление элемента
        public void Add(T item)
        {
            var node = new Node<T>(item); //создаем узел для добавления

            if (_head == null) //если это первый элемент
                _head = node; //то он хед
            else
            {
                //иначе делаем связь между последним элементом до добавляемого
                _tail.Next = node; //для предыдущего элемента следующий тот, который добавляем
                node.Previous = _tail; //для добавляемого элемента предыдущий тот, который был последним
            }

            _tail = node; //если первый - элемент и первый и последний, иначе чисто последнему элементу значение присваиваем

            Count++;
        }

        //удаление элемента
        public bool Remove(T item)
        {
            var current = _head; //создаем узел от начала списка, чтобы можно было пройти по всему списку

            // поиск удаляемого узла
            while (current != null) //пока не дойдем до конца списка
            {
                if (current.Value.Equals(item)) //если элемент соответствует тому, который хотим удалить
                {
                    break; //нужный нам элемент будет в переменной current
                }
                current = current.Next; //если не соответствует - идем дальше
            }

            if (current == null) return false; //не нашли элемента - false

            // если узел не последний
            if (current.Next != null)
            {
                current.Next.Previous = current.Previous; 
                //связываем элементы возле удаляемого (удаляем 3й элемент - связываем 2 и 4)
                // конкретно здесь мы для 4го элемента делаем предыдущм 2й
            }
            else
            {
                // если последний, переустанавливаем tail
                _tail = current.Previous; //последним будет элемент перед удаляемым
            }

            // если узел не первый
            if (current.Previous != null)
            {
                current.Previous.Next = current.Next;
                //связываем элементы возле удаляемого (удаляем 3й элемент - связываем 2 и 4)
                // конкретно здесь мы для 2го элемента делаем следующим 4й
            }
            else
            {
                // если первый, переустанавливаем head
                _head = current.Next; //если элемент первый - делаем следующий после него хедом
            }
            Count--; //уменьшаем количество элементов
            return true; //говорим об успшном удалении
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