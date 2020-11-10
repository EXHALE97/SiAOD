namespace Lab15
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    public class BTree<TK, TP> where TK : IComparable<TK>
    {
        public BTree(int degree)
        {
            if (degree < 2)
            {
                throw new ArgumentException("BTree degree must be at least 2", "degree");
            }

            this.Root = new Node<TK, TP>(degree);
            this.Degree = degree;
            this.Height = 1;
        }

        public Node<TK, TP> Root { get; private set; }//корень

        public int Degree { get; private set; }//степень

        public int Height { get; private set; }//высота


        //Поиск в дереве по ключу
        public Entry<TK, TP> Search(TK key)
        {
            return this.SearchInternal(this.Root, key);//возвращает узел (вызов приватной функции поиска)
        }


        //вставка
        public void Insert(TK newKey, TP newPointer)
        {
            //если есть место в корне
            if (!this.Root.HasReachedMaxEntries)
            {
                this.InsertNonFull(this.Root, newKey, newPointer);//вставка
                return;
            }

            //если родительский узел также был заполнен – то нам опять приходится разбивать.
            //И так далее до корня (если разбивается корень – то появляется новый корень и глубина дерева увеличивается).
            //вставить ключ в уже заполненный лист невозможно => необходима операция разбиения узла на 2
            var oldRoot = this.Root;
            this.Root = new Node<TK, TP>(this.Degree);//создаем новый узел
            this.Root.Children.Add(oldRoot); //перемещаем старый корень
            this.SplitChild(this.Root, 0, oldRoot);//расчепляем узел на 2
            this.InsertNonFull(this.Root, newKey, newPointer); //добалвяем ключ

            this.Height++;//если разбивается корень – то появляется новый корень и глубина дерева увеличивается
        }

        //удаление по ключу (внешний)
        public void Delete(TK keyToDelete)
        {
            this.DeleteInternal(this.Root, keyToDelete);

            // if root's last entry was moved to a child node, remove it
            if (this.Root.Entries.Count == 0 && !this.Root.IsLeaf)
            {
                this.Root = this.Root.Children.Single();
                this.Height--;
            }
        }

        //метод удаления
        private void DeleteInternal(Node<TK, TP> node, TK keyToDelete)
        {
            var i = node.Entries.TakeWhile(entry => keyToDelete.CompareTo(entry.Key) > 0).Count();//ищем позицию

            // нашли ключ в узле - удаляем из него
            if (i < node.Entries.Count && node.Entries[i].Key.CompareTo(keyToDelete) == 0)
            {
                this.DeleteKeyFromNode(node, keyToDelete, i);
                return;
            }

            //иначе удаление из поддерева
            if (!node.IsLeaf)
            {
                this.DeleteKeyFromSubtree(node, keyToDelete, i);
            }
        }

        //удаление ключа из поддерева
        private void DeleteKeyFromSubtree(Node<TK, TP> parentNode, TK keyToDelete, int subtreeIndexInNode)
        {
            
            var childNode = parentNode.Children[subtreeIndexInNode];

            //Если удаление происходит из листа,
            //то необходимо проверить, сколько ключей находится в нем.
            //если существует соседний лист (находящийся рядом с ним и имеющий такого же родителя), который содержит больше t-1 ключа,
            //то выберем ключ из этого соседа, который является разделителем между оставшимися ключами узла-соседа и исходного узла
            if (childNode.HasReachedMinEntries)
            {
                var leftIndex = subtreeIndexInNode - 1;
                var leftSibling = subtreeIndexInNode > 0 ? parentNode.Children[leftIndex] : null;//сосед слева

                var rightIndex = subtreeIndexInNode + 1;
                var rightSibling = subtreeIndexInNode < parentNode.Children.Count - 1 //сосед справа
                                                ? parentNode.Children[rightIndex]
                                                : null;
                
                if (leftSibling != null && leftSibling.Entries.Count > this.Degree - 1)//если сосед слева не пуст и содержит больше t-1 ключа
                {
                    //перемещаем соседа в родительский узел и один узел-разделитель из родительского перемещаем в исходный узел
                    childNode.Entries.Insert(0, parentNode.Entries[subtreeIndexInNode]);//перемещение разделителя
                    parentNode.Entries[subtreeIndexInNode] = leftSibling.Entries.Last();//вставка соседа в родительский
                    leftSibling.Entries.RemoveAt(leftSibling.Entries.Count - 1);//удаление из соседа

                    if (!leftSibling.IsLeaf) 
                    {
                        childNode.Children.Insert(0, leftSibling.Children.Last());
                        leftSibling.Children.RemoveAt(leftSibling.Children.Count - 1);
                    }
                }
                else if (rightSibling != null && rightSibling.Entries.Count > this.Degree - 1) //если сосед справа не пуст и содержит больше t-1 ключа
                {
                    //перемещаем соседа в родительский узел и один узел-разделитель из родительского перемещаем в исходный узел
                    childNode.Entries.Add(parentNode.Entries[subtreeIndexInNode]);//перемещение разделителя
                    parentNode.Entries[subtreeIndexInNode] = rightSibling.Entries.First();//вставка соседа в родительский
                    rightSibling.Entries.RemoveAt(0);//удаление из соседа

                    if (!rightSibling.IsLeaf) //если это не корень, мы выполняем аналогичную процедуру с ним
                    {
                        childNode.Children.Add(rightSibling.Children.First());
                        rightSibling.Children.RemoveAt(0);
                    }
                }
                else
                {
                    //Если же все соседи нашего узла имеют по t-1 ключу. То мы объединяем его с каким-либо соседом, удаляем нужный ключ.
                    //И тот ключ из узла-родителя, который был разделителем для
                    //этих двух «бывших» соседей, переместим в наш новообразовавшийся узел (очевидно, он будет в нем медианой).

                    if (leftSibling != null)
                    {
                        childNode.Entries.Insert(0, parentNode.Entries[subtreeIndexInNode]);
                        var oldEntries = childNode.Entries;
                        childNode.Entries = leftSibling.Entries;
                        childNode.Entries.AddRange(oldEntries);
                        if (!leftSibling.IsLeaf)
                        {
                            var oldChildren = childNode.Children;
                            childNode.Children = leftSibling.Children;
                            childNode.Children.AddRange(oldChildren);
                        }

                        parentNode.Children.RemoveAt(leftIndex);
                        parentNode.Entries.RemoveAt(subtreeIndexInNode);
                    }
                    else
                    {
                        Debug.Assert(rightSibling != null, "Node should have at least one sibling");
                        childNode.Entries.Add(parentNode.Entries[subtreeIndexInNode]);
                        childNode.Entries.AddRange(rightSibling.Entries);
                        if (!rightSibling.IsLeaf)
                        {
                            childNode.Children.AddRange(rightSibling.Children);
                        }

                        parentNode.Children.RemoveAt(rightIndex);
                        parentNode.Entries.RemoveAt(subtreeIndexInNode);
                    }
                }
            }

            
            //Если больше t-1, то просто удаляем и больше ничего делать не нужно. 
            this.DeleteInternal(childNode, keyToDelete);
        }
        
        //удаление ключа из узла
        private void DeleteKeyFromNode(Node<TK, TP> node, TK keyToDelete, int keyIndexInNode)
        {
            //Если корень одновременно является листом, то есть в дереве всего один узел,
            //мы просто удаляем ключ из этого узла.
            if (node.IsLeaf)
            {
                node.Entries.RemoveAt(keyIndexInNode);
                return;
            }

            //предшествующий потомок
            var predecessorChild = node.Children[keyIndexInNode];
            if (predecessorChild.Entries.Count >= this.Degree)//если в нем количество вхождений больше или равно мин. степени
            {
                var predecessor = this.DeletePredecessor(predecessorChild);//удаляем предшественника
                node.Entries[keyIndexInNode] = predecessor;//перемешаем его в исходный узел
            }
            //иначе
            else
            {
                //преемник
                var successorChild = node.Children[keyIndexInNode + 1];
                if (successorChild.Entries.Count >= this.Degree)//если в нем количество вхождений больше или равно мин. степени
                {
                    var successor = this.DeleteSuccessor(predecessorChild);//удаляем преемника
                    node.Entries[keyIndexInNode] = successor; //перемешаем его в исходный узел
                }
                else
                {
                    predecessorChild.Entries.Add(node.Entries[keyIndexInNode]);//иначе добавляем в предшественника все значение исходного узла
                    predecessorChild.Entries.AddRange(successorChild.Entries);//и преемника
                    predecessorChild.Children.AddRange(successorChild.Children);

                    node.Entries.RemoveAt(keyIndexInNode);
                    node.Children.RemoveAt(keyIndexInNode + 1);

                    this.DeleteInternal(predecessorChild, keyToDelete);//удаление предшественника
                }
            }
        }

        //удаление предшественника (вспомогательный метод)
        private Entry<TK, TP> DeletePredecessor(Node<TK, TP> node)
        {
            if (node.IsLeaf)//если лист
            {
                var result = node.Entries[node.Entries.Count - 1];
                node.Entries.RemoveAt(node.Entries.Count - 1);//удаляем последнее входное значение и возвращаем его
                return result;
            }
            //иначе рекурсивно переходим к потомку
            return this.DeletePredecessor(node.Children.Last());
        }

        //удаление преемника (вспомогательный метод)
        private Entry<TK, TP> DeleteSuccessor(Node<TK, TP> node)
        {
            if (node.IsLeaf) //если лист
            {
                var result = node.Entries[0];
                node.Entries.RemoveAt(0); //удаляем первое входное узначение и возвращаем его
                return result;
            }
            //иначе рекурсивно переходим к потомку
            return this.DeletePredecessor(node.Children.First());
        }

        //поиск пл ключу
        private Entry<TK, TP> SearchInternal(Node<TK, TP> node, TK key)
        {
            //начиная с корня просматриваем ключи
            var i = node.Entries.TakeWhile(entry => key.CompareTo(entry.Key) > 0).Count(); 

            if (i < node.Entries.Count && node.Entries[i].Key.CompareTo(key) == 0)
            {
                //если нашли нужное знаение - возврвщаем
                return node.Entries[i];
            }

            //если это был лист без потомков (дошли до конца) - ничего не нашли
            //если еще не дошли - продолжаем поиск в потомке
            return node.IsLeaf ? null : this.SearchInternal(node.Children[i], key);
        }

        //разделяет узел на 2
        private void SplitChild(Node<TK, TP> parentNode, int nodeToBeSplitIndex, Node<TK, TP> nodeToBeSplit)
        {
            //t (this.Degree) -минимальная степень
            //разбиваем на 2 по t-1, а средний элемент (для которого t-1 первых ключей меньше его, а t-1 последних больше)
            //перемещается в родительский узел.
            var newNode = new Node<TK, TP>(this.Degree);

            parentNode.Entries.Insert(nodeToBeSplitIndex, nodeToBeSplit.Entries[this.Degree - 1]);
            parentNode.Children.Insert(nodeToBeSplitIndex + 1, newNode);

            newNode.Entries.AddRange(nodeToBeSplit.Entries.GetRange(this.Degree, this.Degree - 1));
            
            // remove also Entries[this.Degree - 1], which is the one to move up to the parent
            nodeToBeSplit.Entries.RemoveRange(this.Degree - 1, this.Degree);

            
            if (!nodeToBeSplit.IsLeaf)
            {
                newNode.Children.AddRange(nodeToBeSplit.Children.GetRange(this.Degree, this.Degree));
                nodeToBeSplit.Children.RemoveRange(this.Degree, this.Degree);
            }
        }

        //метод вставки
        private void InsertNonFull(Node<TK, TP> node, TK newKey, TP newPointer)
        {
            var positionToInsert = node.Entries.TakeWhile(entry => newKey.CompareTo(entry.Key) >= 0).Count();//позиция для вставки

            // если это вершина
            if (node.IsLeaf)
            {
                node.Entries.Insert(positionToInsert, new Entry<TK, TP>() { Key = newKey, Pointer = newPointer });//вствка элемента в лист
                return;
            }

            // иначе
            var child = node.Children[positionToInsert];
            if (child.HasReachedMaxEntries) //если лист заполнен
            {
                this.SplitChild(node, positionToInsert, child); //расщепляем на 2
                if (newKey.CompareTo(node.Entries[positionToInsert].Key) > 0)
                {
                    positionToInsert++; //вставляем по возрастанию (сравниваем знаения). Если наше значение больше - позиция для вставки увеличивется
                }
            }

            this.InsertNonFull(node.Children[positionToInsert], newKey, newPointer); //идем от родителя к листам дальше
        }

        public void PrintTree(Node<TK, TP> root, string indent = "")
        {
            if (!root.IsLeaf)//если есть потомки
            {
                Console.Write($"{indent}[Root] - ");
                foreach (var value in root.Entries) //пишем root, выводим все значения внутри
                {
                    if (root.Entries.Last().Key.Equals(value.Key))
                    {
                        Console.Write(value.Key + ".\n");
                    }
                    else
                    {
                        Console.Write(value.Key.ToString() + ',');
                    }
                }

                foreach (var leaf in root.Children)
                {
                    PrintTree(leaf, indent + "   "); //для каждого потомка запускаем вывод на экран
                }
            }
            else
            {
                if (Root.Equals(root)) //если нет потомков и это корень
                {
                    Console.Write($"{indent}[Root] - ");
                    foreach (var value in root.Entries) //выводим root + все элементы
                    {
                        if (root.Entries.Last().Key.Equals(value.Key))
                        {
                            Console.Write(value.Key + ".\n");
                        }
                        else
                        {
                            Console.Write(value.Key.ToString() + ',');
                        }
                    }
                }
                else //если это лист
                {
                    Console.Write($"{indent}[Leaf] - ");
                    foreach (var value in root.Entries) //выводим Leaf и все элементы
                    {
                        if (root.Entries.Last().Key.Equals(value.Key))
                        {
                            Console.Write(value.Key + ".\n");
                        }
                        else
                        {
                            Console.Write(value.Key.ToString() + ',');
                        }
                    }
                }
            }
            
        }
    }
}
