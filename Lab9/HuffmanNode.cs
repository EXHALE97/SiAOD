using System;

namespace Lab9
{
    class HuffmanNode : IComparable<HuffmanNode>
    {
        public string symbol;   // символ
        public double frequency;          // частота
        public string code;            // код
        public HuffmanNode parentNode; // родительский узел
        public HuffmanNode leftTree;   // левый узел
        public HuffmanNode rightTree;  // правый узел
        public bool isLeaf;            // лист или нет


        public HuffmanNode(string value, double frequency)    // создание узла
        {
            symbol = value;     // символ
            this.frequency = frequency;      // вероятность
            rightTree = leftTree = parentNode = null;       // на данный момент нет родителей/левых/правых узлов
            code = "";          // код
            isLeaf = true;      // true потому что изначально нет предков
        }


        public HuffmanNode(HuffmanNode node1, HuffmanNode node2) // объединение узлов
        {
            code = "";
            isLeaf = false;
            parentNode = null;

            
            //если частота у 1 узла больше 2, то 1 будет справа, а  2 - слева
            if (node1.frequency >= node2.frequency)
            {
                rightTree = node1;//node1 - справа
                leftTree = node2;//node2 - слева
                rightTree.parentNode = leftTree.parentNode = this;     // this = новый узел - установили новый узел родителем для этих двух
                symbol = node1.symbol + node2.symbol;//символ нового узла равен обхединению входных узлов
                frequency = node1.frequency + node2.frequency;//тоже самое с частотой
            }
            //иначе тоже самое, только стороны другие
            else if (node1.frequency < node2.frequency)
            {
                rightTree = node2;
                leftTree = node1;
                leftTree.parentNode = rightTree.parentNode = this;
                symbol = node2.symbol + node1.symbol;
                frequency = node2.frequency + node1.frequency;
            }
        }


        // метод сравнения. Сравниваем частоты у каждого узла
        public int CompareTo(HuffmanNode otherNode) 
        {
            return frequency.CompareTo(otherNode.frequency);
        }
    }
}
