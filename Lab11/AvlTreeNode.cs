using System;

namespace Lab11
{
    public class AvlTreeNode<TNode> : IComparable<TNode> where TNode : IComparable
    {
        AvlTree<TNode> _tree;

        AvlTreeNode<TNode> _left;   // левый  потомок
        AvlTreeNode<TNode> _right;  // правый потомок


        public AvlTreeNode(TNode value, AvlTreeNode<TNode> parent, AvlTree<TNode> tree)
        {
            Value = value; //значение
            Parent = parent; //родитель
            _tree = tree; //дерево
        }


        public AvlTreeNode<TNode> Left
        {
            get => _left; //получение левого узла

            internal set //установка левого узла
            {
                _left = value; 

                if (_left != null) //если левый не пустой
                {
                    _left.Parent = this;  // установка указателя на родительский элемент
                }
            }
        }

        public AvlTreeNode<TNode> Right
        {
            get => _right; //установка правого узла

            internal set
            {
                _right = value;
                 
                if (_right != null) //если правый не пустой
                {
                    _right.Parent = this; // установка указателя на родительский элемент
                }
            }
        }

        // Указатель на родительский узел

        public AvlTreeNode<TNode> Parent
        {
            get; //получение
            internal set;//установка значения
        }

        // значение текущего узла 

        public TNode Value
        {
            get; //получение
        }

        // Сравнивает текущий узел по указаному значению, возвращет 1, если значение экземпляра больше переданного значения,  
        // возвращает -1, когда значение экземпляра меньше переданого значения, 0 - когда они равны.     
        public int CompareTo(TNode other)
        {
            return Value.CompareTo(other);
        }


        internal void Balance() //балансировка
        {
            if (State == TreeState.RightHeavy) //если перевешивает правая сторона
            {
                if (Right != null && Right.BalanceFactor < 0) //если справа не пусто и правое поддерево сбалансировано
                {
                    LeftRightRotation(); //вращение влево, а потом вправо
                }

                else
                {
                    LeftRotation(); //иначе вращение влево
                }
            }
            else if (State == TreeState.LeftHeavy) //если перевешивает левая сторона
            {
                if (Left != null && Left.BalanceFactor > 0) //если слева не пусто и левое поддерево сбалансировано
                {
                    RightLeftRotation(); //вращение вправо, а потом влево
                }
                else //иначе вращение вправо
                {
                    RightRotation();
                }
            }
        }
        private int MaxChildHeight(AvlTreeNode<TNode> node) //подсчет высоты дерева
        {
            if (node != null) //если узел не пустой
            {
                return 1 + Math.Max(MaxChildHeight(node.Left), MaxChildHeight(node.Right)); //рекурсивно считаем высоту с каждой стороны узла
            }

            return 0; //иначе 0
        }

        private int LeftHeight => MaxChildHeight(Left); //количество потомков слева

        private int RightHeight => MaxChildHeight(Right); //количество потомков справа

        private TreeState State //получение состояния
        {
            get
            {
                if (LeftHeight - RightHeight > 1)
                {
                    return TreeState.LeftHeavy;  //перевешивает слева
                }

                if (RightHeight - LeftHeight > 1)
                {
                    return TreeState.RightHeavy; //перевешивает справа
                }

                return TreeState.Balanced; //сбалансированное
            }
        }


        private int BalanceFactor => RightHeight - LeftHeight;

        private enum TreeState //состояния дерева
        {
            Balanced,
            LeftHeavy,
            RightHeavy,
        }

        private void LeftRotation() //левое вращение
        {

            // До
            //     12(this)     
            //      \     
            //       15     
            //        \     
            //         25     
            //     
            // После     
            //       15     
            //      / \     
            //     12  25  

            // Сделать правого потомка новым корнем дерева.
            AvlTreeNode<TNode> newRoot = Right;
            ReplaceRoot(newRoot);

            // Поставить на место правого потомка - левого потомка нового корня.    
            Right = newRoot.Left;
            // Сделать текущий узел - левым потомком нового корня.    
            newRoot.Left = this;
        }


        private void RightRotation()//правое вращение
        {
            // Было
            //     c (this)     
            //    /     
            //   b     
            //  /     
            // a     
            //     
            // Стало    
            //       b     
            //      / \     
            //     a   c  

            // Левый узел текущего элемента становится новым корнем
            AvlTreeNode<TNode> newRoot = Left;
            ReplaceRoot(newRoot);

            // Перемещение правого потомка нового корня на место левого потомка старого корня
            Left = newRoot.Right;

            // Правым потомком нового корня, становится старый корень.     
            newRoot.Right = this;
        }

        private void LeftRightRotation() //вращение влево, а потом вправо (смешанное)
        {
            // Было
            //     a (this)     
            //      \    
            //       c   
            //      /     
            //     b     
            //    
            // Стало    
            //       b     
            //      / \     
            //     a   c  
            Right.RightRotation(); //правое вращение для правой ветки
            LeftRotation(); //левое вращение для конкретного узла
        }


        private void RightLeftRotation()//вращение вправо, а потом влево(смешанное)
        {

            // Было
            //      c (this)     
            //     /    
            //    a     
            //     \     
            //      b     
            //    
            // Стало    
            //       b     
            //      / \     
            //     a   c  
            Left.LeftRotation(); //левое вращение для левой ветки
            RightRotation();//правое вращение для конкретного узла
        }

        //Перемещение корня
        private void ReplaceRoot(AvlTreeNode<TNode> newRoot)
        {
            if (Parent != null) //если родитель не пустой
            {
                if (Parent.Left == this) //замена левого потомка
                {
                    Parent.Left = newRoot;
                }
                else if (Parent.Right == this) //замена правого потомка
                {
                    Parent.Right = newRoot;
                }
            }
            else //иначе
            {
                _tree.Head = newRoot; //корень дерева - входное значение
            }

            newRoot.Parent = Parent; //родитель входного знаения - текущий родитель
            Parent = newRoot; //родитель текущего узла - входной узел
        }
    }
}