namespace Lab3
{
    //узел в списке
    public class Node<T>
    {
        public Node<T> Previous { get; set; } //предыдущий элемент
        public Node<T> Next { get; set; } //ссылка на следующий элемент
        public T Value { get; set; } //значение конкретного элемента

        public Node(T value)
        {
            Value = value;
        }
    }
}