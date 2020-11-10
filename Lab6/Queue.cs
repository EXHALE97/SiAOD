using System;
using System.Collections;
using System.Collections.Generic;

namespace Lab6
{
    public class Queue<T> : IEnumerable<T>
    {
        private Node<T> _head; // головной/первый элемент
        private Node<T> _tail; // последний/хвостовой элемент

        // добавление в очередь
        public void Enqueue(T data)
        {
            var node = new Node<T>(data); //узел с данными
            var tempNode = _tail;//хвостовой узел
            _tail = node;//делаем последним элементом тот, который добавляем
            if (Count == 0) //если это первый элемент
                _head = _tail; //то делаем его и головным (хвост == голове)
            else
                tempNode.Next = _tail;//иначе для последнего узла делаем следующим тот, который добавляем
            Count++; //увеличиаем количество элементов
        }
        // удаление из очереди
        public T Dequeue()
        {
            if (Count == 0) //если пустая очередь
                throw new InvalidOperationException(); //ошибка
            var output = _head.Value; //элемент, который удаляем
            _head = _head.Next;//делаем головным элементом следующий
            Count--;//уменьшаем количество элементов
            return output; //возвращаем элемент, который удалили
        }
        // получаем первый элемент
        public T First
        {
            get
            {
                if (IsEmpty) //если пытаемся получить первый элемент из пустой очереди
                    throw new InvalidOperationException(); //ошибка
                return _head.Value;//иначе возвращаем головной элемент
            }
        }
        // получаем последний элемент
        public T Last
        {
            get
            {
                if (IsEmpty)//если пытаемся получить первый элемент из пустой очереди
                    throw new InvalidOperationException(); //ошибка
                return _tail.Value; //иначе возвращаем хвостовой элемент
            }
        }
        public int Count { get; private set; } //количество элементов очереди

        public bool IsEmpty => Count == 0; //проверка на наличие элементов

        public void Clear() //очистить очередь
        {
            _head = null;
            _tail = null;
            Count = 0;
        }

        public bool Contains(T data) //проверка наличия элемента
        {
            var current = _head;
            while (current != null) //проходим по всем элементам
            {
                if (current.Value.Equals(data)) //если найден - true
                    return true;
                current = current.Next;// или переходим к следующему
            }
            return false;//если не найдено совпадений - false
        }

        //реализуем интерфейс IEnumberable чтобы можно было обращаться к элементам по индексам
        //в случае с очередью для того, чтобы можно было пройти по всем элементам через foreach
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        //просто через yield return получаем все элементы списка. Нужно чисто чтобы использовать интерфейс IEnumberable
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            var current = _head; //создаем узел чтобы пройти по всем элементам от начала
            while (current != null) //пока не дошли до конца
            {
                yield return current.Value; //возвращаем каждый имеющийся элемент
                current = current.Next; //идем дальше
            }
        }
    }
}