using System;

namespace Lab8
{
    //Расположение узла относительно родителя
    public enum Side
    {
        Left,
        Right
    }

    //Узел бинарного дерева
    public class Node<T> where T : IComparable
    {
        public Node(T data)
        {
            Data = data;
        }

        public T Data { get; set; } // Данные которые хранятся в узле

        public Node<T> LeftNode { get; set; } // Левая ветка

        public Node<T> RightNode { get; set; } // Правая ветка

        public Node<T> ParentNode { get; set; } // Родитель

        // Расположение узла относительно его родителя
        public Side? NodeSide =>
            ParentNode == null //если нет родителя
                ? (Side?)null //нет стороны
                : ParentNode.LeftNode == this //если для родителя этот узел слева
                    ? Side.Left //сторона левая
                    : Side.Right; //сторона правая

        // Преобразование экземпляра класса в строку
        public override string ToString() => Data.ToString();
    }
}