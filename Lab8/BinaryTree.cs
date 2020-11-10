using System;

namespace Lab8
{
    // Бинарное дерево
    public class BinaryTree<T> where T : IComparable
    {
        //Корень бинарного дерева
        public Node<T> RootNode { get; set; }

        // Добавление нового узла в бинарное дерево
        public Node<T> Add(Node<T> node, Node<T> currentNode = null)
        {
            if (RootNode == null) //если дерево пустое
            {
                node.ParentNode = null; //то для добавляемого узла нет родителя
                return RootNode = node; //а корневым становится добавляемый
            }

            currentNode ??= RootNode; //добавляемый становится текущим, если тот пустой
            node.ParentNode = currentNode; //родитель добавляемого узла - текущий узел
            int result;
            //определяем с какой стороны вставить элемент
            //и возвращаем добавленный элемент
            return (result = node.Data.CompareTo(currentNode.Data)) == 0 //сравнимаем данные добавляемого узла и текущего
                //если разницы нет - элемент не добавляется
                ? currentNode
                : result < 0 //если добавляемый элемент меньше текущего
                    //вставляем его слева
                    ? currentNode.LeftNode == null //если у текущего элемента слева пусто
                        ? (currentNode.LeftNode = node) //делаем добавляемый узел левым для текущего
                        : Add(node, currentNode.LeftNode) //иначе добавляем элемент под левый от текущего
                    //иначе вставляем его справа
                    : currentNode.RightNode == null//если у текущего элемента справа пусто
                        ? (currentNode.RightNode = node)//делаем добавляемый узел правым для текущего
                        : Add(node, currentNode.RightNode); //иначе добавляем элемент под правый от текущего
        }

        // Поиск узла по значению
        public Node<T> FindNode(T data, Node<T> startWithNode = null)
        {
            startWithNode ??= RootNode; //если не указан элемент, от которого ищем - считается, что ищем от корня
            int result;
            return (result = data.CompareTo(startWithNode.Data)) == 0 //сравниваем искомые данные с элементом, от которого ищем
                //если разницы нет
                ? startWithNode //возвращаем элемент, от которого начали поиск
                //иначе
                : result < 0 //если тот, который мы ищем, меньше текущего
                    //продолжаем поиск в левой ветке
                    ? startWithNode.LeftNode == null //если слева ничего нет
                        ? null //ничего не нашли - возвращаем пустое значение
                        : FindNode(data, startWithNode.LeftNode) //ищем дальше
                    //иначе продолжаем поиск в правой ветке
                    : startWithNode.RightNode == null
                        ? null //ничего не нашли - возвращаем пустое значение
                        : FindNode(data, startWithNode.RightNode); //ищем дальше
        }

        // Удаление узла бинарного дерева
        private void Remove(Node<T> node)
        {
            if (node == null) //если передали пустое значение
            {
                return; //выход из функции, ничего не удаляется
            }

            var currentNodeSide = node.NodeSide; //сторона удаляемого узла относительно родительского
            //если у узла нет подузлов, можно его удалить
            if (node.LeftNode == null && node.RightNode == null)
            {
                if (currentNodeSide == Side.Left) //если сторона левая
                {
                    node.ParentNode.LeftNode = null; //для родителя левый узел стал пустой
                }
                else //если сторона правая
                {
                    node.ParentNode.RightNode = null; //для родителя правый узел стал пустой
                }
            }
            //если нет левого, то правый ставим на место удаляемого 
            else if (node.LeftNode == null)
            {
                if (currentNodeSide == Side.Left) //если сторона левая
                {
                    node.ParentNode.LeftNode = node.RightNode; //родитель левого узла для удаляемого становится правым для удаляемого
                }
                else //если сторона правая
                {
                    node.ParentNode.RightNode = node.RightNode; //родитель правого узла для удаляемого становится правым для удаляемого
                }

                node.RightNode.ParentNode = node.ParentNode; //родитель правого узла для удаляемого становится родителем удаляемого
            }
            //если нет правого, то левый ставим на место удаляемого 
            else if (node.RightNode == null)
            {
                if (currentNodeSide == Side.Left) //если сторона левая
                {
                    node.ParentNode.LeftNode = node.LeftNode; //родитель левого узла для удаляемого становится левым для удаляемого
                }
                else //если сторона правая
                {
                    node.ParentNode.RightNode = node.LeftNode;//родитель правого узла для удаляемого становится левым для удаляемого
                }

                node.LeftNode.ParentNode = node.ParentNode; //родитель левого узла для удаляемого становится родителем удаляемого
            }
            //если оба дочерних присутствуют, 
            //то правый становится на место удаляемого,
            //а левый вставляется в правый
            else
            {
                switch (currentNodeSide)
                {
                    case Side.Left: //если у удаляемого узла сторона левая относительно родителя
                        node.ParentNode.LeftNode = node.RightNode; //правый узел удаляемого элемента становится на место удаляемого
                        node.RightNode.ParentNode = node.ParentNode; //для левого узла родителем становится родитель удаляемого
                        Add(node.LeftNode, node.RightNode); //преобразование дерева с учетом удаления
                        break;
                    case Side.Right: // если у удаляемого узла сторона левая относительно родителя
                        node.ParentNode.RightNode = node.RightNode;//левый узел удаляемого элемента становится на место удаляемого
                        node.RightNode.ParentNode = node.ParentNode;//для правого узла родителем становится родитель удаляемого
                        Add(node.LeftNode, node.RightNode); //преобразование дерева с учетом удаления
                        break;
                    default: //если сторона не определена
                        var bufLeft = node.LeftNode; //создание временных хранилищ узлов
                        var bufRightLeft = node.RightNode.LeftNode;
                        var bufRightRight = node.RightNode.RightNode;
                        node.Data = node.RightNode.Data; //присваиваем удаляемому узлу значение правого
                        //грубо говоря, меняем правый и левый узлы местами
                        node.RightNode = bufRightRight;//правый узел для удаляемого будет левый узел правого (слева направо и вверх поднимаем)
                        node.LeftNode = bufRightLeft;//левый узел для удаляемого будет правый узел правого (справа налево и вверх поднимаем)
                        Add(bufLeft, node); //преобразование дерева с учетом удаления
                        break;
                }
            }
        }

        //вывод дерева на экран
        private void PrintTree(Node<T> startNode, string indent = "", Side? side = null)
        {
            if (startNode != null) //если начальный узел не пустой
            {
                var nodeSide = side == null ? "+" : side == Side.Left ? "L" : "R";//вывод текста в зависимости от ветки (стороны)
                // "+" - корень, L/R - сторона
                Console.WriteLine($"{indent} [{nodeSide}]- {startNode.Data}");//оформление вывода
                indent += new string(' ', 3);//увеличение отступа
                //рекурсивный вызов для левой и правой веток
                PrintTree(startNode.LeftNode, indent, Side.Left); 
                PrintTree(startNode.RightNode, indent, Side.Right);
            }
        }

        //метод для непосредственного добавления (публичный, видно из других классов, не требует доп данных)
        public Node<T> Add(T data)
        {
            return Add(new Node<T>(data));
        }

        //метод для непосредственного удаления (публичный, видно из других классов, не требует доп данных)
        public void Remove(T data)
        {
            var foundNode = FindNode(data); //поиск узла
            Remove(foundNode);//удаление
        }

        //вывод бинарного дерева (публичный, видно из других классов, не требует доп данных)
        public void PrintTree()
        {
            PrintTree(RootNode);
        }
    }
}