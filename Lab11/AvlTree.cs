using System;
using System.Collections.Generic;

namespace Lab11
{
    public class AvlTree<T> : IEnumerable<T> where T : IComparable

    {
        // Свойство для корня дерева

        public AvlTreeNode<T> Head
        {
            get;
            internal set;
        }


        public int Count
        {
            get;
            private set;
        }



        // Метод добавлет новый узел

        public void Add(T value)
        {
            // Вариант 1:  Дерево пустое - создание корня дерева      
            if (Head == null)
            {
                Head = new AvlTreeNode<T>(value, null, this);
            }

            // Вариант 2: Дерево не пустое - найти место для добавление нового узла.

            else
            {
                AddTo(Head, value);
            }

            Count++;
        }

        // Алгоритм рекурсивного добавления нового узла в дерево.

        private void AddTo(AvlTreeNode<T> node, T value)
        {
            // Вариант 1: Добавление нового узла в дерево. Значение добавлемого узла меньше чем значение текущего узла.      

            if (value.CompareTo(node.Value) < 0)
            {
                //Создание левого узла, если его нет.

                if (node.Left == null)
                {
                    node.Left = new AvlTreeNode<T>(value, node, this);
                }

                else
                {
                    // Переходим к следующему левому узлу
                    AddTo(node.Left, value);
                }
            }
            // Вариант 2: Добавлемое значение больше или равно текущему значению.

            else
            {
                //Создание правого узла, если его нет.         
                if (node.Right == null)
                {
                    node.Right = new AvlTreeNode<T>(value, node, this);
                }
                else
                {
                    // Переход к следующему правому узлу.             
                    AddTo(node.Right, value);
                }
            }
            //node.Balance();
        }



        public bool Contains(T value) //содержит ли дерево конкретное значние
        {
            return Find(value) != null; //если функция поиска вернуло null, то тогда дерево НЕ содержит значение
        }


        // Находит и возвращает первый узел который содержит искомое значение.
        // Если значение не найдено, возвращает null. 
        // Так же возвращает родительский узел.

        private AvlTreeNode<T> Find(T value)
        {

            var current = Head; // помещаем текущий элемент в корень дерева

            // Пока текщий узел на пустой 
            while (current != null)
            {
                var result = current.CompareTo(value); // сравнение значения текущего элемента с искомым значением

                if (result > 0)
                {
                    // Если значение меньшне текущего - переход влево 
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // Если значение больше текщего - переход вправо             
                    current = current.Right;
                }
                else
                {
                    // Элемент найден      
                    break;
                }
            }
            return current;
        }


        //удаление элемента
        public bool Remove(T value)
        {
            var current = Find(value); //ищем значение

            if (current == null) // узел не найден
            {
                return false;
            }

            var treeToBalance = current.Parent; // баланс дерева относительно узла родителя
            Count--;                                       // уменьшение колиества узлов

            // Вариант 1: Если удаляемый узел не имеет правого потомка      

            if (current.Right == null) // если нет правого потомка
            {
                if (current.Parent == null) // удаляемый узел является корнем
                {
                    Head = current.Left;    // на место корня перемещаем левого потомка

                    if (Head != null)
                    {
                        Head.Parent = null; // убераем ссылку на родителя  
                    }
                }
                else // удаляемый узел не является корнем
                {
                    var result = current.Parent.CompareTo(current.Value);

                    if (result > 0)
                    {
                        // Если значение родительского узла больше значения удаляемого,
                        // сделать левого потомка удаляемого узла, левым потомком родителя.  

                        current.Parent.Left = current.Left;
                    }
                    else if (result < 0)
                    {

                        // Если значение родительского узла меньше чем удаляемого,                 
                        // сделать левого потомка удаляемого узла - правым потомком родительского узла.                 

                        current.Parent.Right = current.Left;
                    }
                }
            }

            // Вариант 2: Если правый потомок удаляемого узла не имеет левого потомка, тогда правый потомок удаляемого узла
            // становится потомком родительского узла.      

            else if (current.Right.Left == null) // если у правого потомка нет левого потомка
            {
                current.Right.Left = current.Left;

                if (current.Parent == null) // текущий элемент является корнем
                {
                    Head = current.Right;

                    if (Head != null)
                    {
                        Head.Parent = null;
                    }
                }
                else
                {
                    var result = current.Parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        // Если значение узла родителя больше чем значение удаляемого узла,                 
                        // сделать правого потомка удаляемого узла, левым потомком его родителя.                 

                        current.Parent.Left = current.Right;
                    }

                    else if (result < 0)
                    {
                        // Если значение родительского узла меньше значения удаляемого,                 
                        // сделать правого потомка удаляемого узла - правым потомком родителя.                 

                        current.Parent.Right = current.Right;
                    }
                }
            }

            // Вариант 3: Если правый потомок удаляемого узла имеет левого потомка,      
            // заместить удаляемый узел, крайним левым потомком правого потомка.     
            else
            {
                // Нахожление крайнего левого узла для правого потомка удаляемого узла.       

                AvlTreeNode<T> leftmost = current.Right.Left;

                while (leftmost.Left != null)
                {
                    leftmost = leftmost.Left;
                }

                // Родительское правое поддерево становится родительским левым поддеревом.         

                leftmost.Parent.Left = leftmost.Right;

                // Присвоить крайнему левому узлу, ссылки на правого и левого потомка удаляемого узла.         
                leftmost.Left = current.Left;
                leftmost.Right = current.Right;

                if (current.Parent == null)
                {
                    Head = leftmost;

                    if (Head != null)
                    {
                        Head.Parent = null;
                    }
                }
                else
                {
                    var result = current.Parent.CompareTo(current.Value);

                    if (result > 0)
                    {
                        // Если значение родительского узла больше значения удаляемого,                 
                        // сделать крайнего левого потомка левым потомком родителя удаляемого узла.                 

                        current.Parent.Left = leftmost;
                    }
                    else if (result < 0)
                    {
                        // Если значение родительского узла, меньше чем значение удаляемого,                 
                        // сделать крайнего левого потомка, правым потомком родителя удаляемого узла.                 

                        current.Parent.Right = leftmost;
                    }
                }
            }

            if (treeToBalance != null)
            {
                treeToBalance.Balance();
            }

            else
            {
                Head?.Balance();
            }

            return true;

        }


        public void Clear()
        {
            Head = null; // удаление дерева
            Count = 0;
        }


        public IEnumerator<T> InOrderTraversal()
        {

            // рекурсивное перемещение по дереву

            if (Head != null) // существует ли корень дерева
            {

                var stack = new Stack<AvlTreeNode<T>>();
                var current = Head;

                // при рекурсивном перемещении по дереву, нужно указывать какой потомок будет слудеющим (правый или левый)

                var goLeftNext = true;

                // Начинаем с помещения корня в стек
                stack.Push(current);

                while (stack.Count > 0)
                {
                    // Если перемещаемся влево ... 
                    if (goLeftNext)
                    {
                        // Перемещение всех левых потомков в стек.

                        while (current.Left != null)
                        {
                            stack.Push(current);
                            current = current.Left;
                        }
                    }

                    yield return current.Value;

                    // Если перемещаемся вправо 

                    if (current.Right != null)
                    {
                        current = current.Right;

                        // Идинажды перемещаемся вправо, после чего опять идем влево. 

                        goLeftNext = true;
                    }
                    else
                    {
                        // Если перейти вправо нельзя - извлекаем родительский узел. 

                        current = stack.Pop();
                        goLeftNext = false;
                    }
                }
            }
        }

        public void PrintTree(AvlTreeNode<T> startNode, string indent = "")
        {
            if (startNode != null) //если начальный узел не пустой
            {
                if (startNode.Parent != null) //если родитель входного эемента не пустой
                {
                    if (startNode.Parent.Left != null && startNode.Parent.Left.Value.Equals(startNode.Value)) //если сторона узла левая
                    {
                        Console.WriteLine($"{indent} [L]- {startNode.Value}");
                    }
                    else if (startNode.Parent.Right != null && startNode.Parent.Right.Value.Equals(startNode.Value)) //если сторона узла правая
                    {
                        Console.WriteLine($"{indent} [R]- {startNode.Value}");
                    }
                }
                else //иначе это корень
                {
                    Console.WriteLine($"{indent} [+]- {startNode.Value}");
                }

                indent += new string(' ', 3); //увеличиваем отступ
                PrintTree(startNode.Left, indent); //рекурсивно выводим левую и правую ветки
                PrintTree(startNode.Right, indent);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {

            return GetEnumerator();

        }
    }
}