namespace Lab15
{
    using System.Collections.Generic;

    public class Node<TK, TP> //узлы дерева
    {
        private int degree;

        public Node(int degree)
        {
            this.degree = degree;
            this.Children = new List<Node<TK, TP>>(degree);
            this.Entries = new List<Entry<TK, TP>>(degree);
        }

        public List<Node<TK, TP>> Children { get; set; }//входящие листы

        public List<Entry<TK, TP>> Entries { get; set; }//входящие значения в листе

        public bool IsLeaf => this.Children.Count == 0; //лист или нет

        public bool HasReachedMaxEntries => this.Entries.Count == (2 * this.degree) - 1; //достиг ли максимального количества входных значений

        public bool HasReachedMinEntries => this.Entries.Count == this.degree - 1;//достиг ли минимального количества входных значений
    }
}
