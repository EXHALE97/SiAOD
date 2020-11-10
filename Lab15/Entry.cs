namespace Lab15
{
    using System;

    public class Entry<TK, TP> : IEquatable<Entry<TK, TP>>//узлы листа
    {
        public TK Key { get; set; }//ключ

        public TP Pointer { get; set; }//указатель

        public bool Equals(Entry<TK, TP> other) //функция эквивалентности значений ключей
        {
            return this.Key.Equals(other.Key) && this.Pointer.Equals(other.Pointer);
        }
    }
}
