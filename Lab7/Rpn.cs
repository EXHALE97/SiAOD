using System;
using System.Collections.Generic;
using System.Linq;
using static System.Char;

namespace Lab7
{
    public static class Rpn //(ОПЗ)
    {
        //преобразовывает строку в постфиксную запись
        public static string CalculateToPostfix(string input)
        {
            if (DoesStringContainRestrictedSymbols(input)) //проверка на запрещенные символы
            {
                throw new ArgumentException("String contains restricted symbols!");
            }
            return GetExpression(input); //Преобразовываем выражение в постфиксную запись и Возвращаем результат
        }

        //преобразовывает строку в префиксную
        public static string CalculateToPrefix(string input)
        {
            if (DoesStringContainRestrictedSymbols(input)) //проверка на запрещенные символы
            {
                throw new ArgumentException("String contains restricted symbols!");
            }
            //переписывем выражение справа налево
            //преобразуем его в постфиксную запись
            //снова переписывем выражение справа налево и возвращаем его
            return ReverseString(GetExpression(ReverseString(input)));
        }

        //реверсирует строку (меняет порядок букв)
        private static string ReverseString(string input)
        {
            var sReverse = input.ToCharArray(); 
            Array.Reverse(sReverse);//реверс строки
            var newString = new string(sReverse);
            //замена скобок (открытые на закрытые и наоборот)
            newString = newString.Replace('(', '&').Replace(')', '?').Replace('&', ')').Replace('?', '(');
            return newString;
        }

        //Метод, преобразующий входную строку с выражением в постфиксную запись
        private static string GetExpression(string input)
        {
            var output = string.Empty; //Строка для хранения выражения
            var operStack = new Stack<char>(); //Стек для хранения операторов

            for (var i = 0; i < input.Length; i++) //Для каждого символа в входной строке
            {
                //Разделители пропускаем
                if (IsDelimeter(input[i]))
                    continue; //Переходим к следующему символу

                //Если символ - цифра/буква, то считываем все число
                if (IsDigit(input[i]) || IsLetter(input[i])) //Если цифра/буква
                {
                    //Читаем до разделителя или оператора, что бы получить число
                    while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                    {
                        output += input[i]; //Добавляем каждую цифру числа к нашей строке
                        i++; //Переходим к следующему символу

                        if (i == input.Length) break; //Если символ - последний, то выходим из цикла
                    }

                    output += " "; //Дописываем после числа пробел в строку с выражением
                    i--; //Возвращаемся на один символ назад, к символу перед разделителем
                }

                //Если символ - оператор
                if (IsOperator(input[i])) //Если оператор
                {
                    if (input[i] == '(') //Если символ - открывающая скобка
                        operStack.Push(input[i]); //Записываем её в стек
                    else if (input[i] == ')') //Если символ - закрывающая скобка
                    {
                        //Выписываем все операторы до открывающей скобки в строку
                        char s = operStack.Pop();

                        while (s != '(')
                        {
                            output += s.ToString() + ' ';
                            s = operStack.Pop();
                        }
                    }
                    else //Если любой другой оператор
                    {
                        if (operStack.Count > 0) //Если в стеке есть элементы
                            if (GetPriority(input[i]) <= GetPriority(operStack.Peek())) //И если приоритет нашего оператора меньше или равен приоритету оператора на вершине стека
                                output += operStack.Pop().ToString() + " "; //То добавляем последний оператор из стека в строку с выражением

                        operStack.Push(Parse(input[i].ToString())); //Если стек пуст, или же приоритет оператора выше - добавляем операторов на вершину стека

                    }
                }
            }

            //Когда прошли по всем символам, выкидываем из стека все оставшиеся там операторы в строку
            while (operStack.Count > 0)
                output += operStack.Pop() + " ";

            return output; //Возвращаем выражение в постфиксной записи
        }

        //Метод возвращает true, если проверяемый символ - разделитель ("пробел" или "равно")
        private static bool IsDelimeter(char c)
        {
            if ((" =".IndexOf(c) != -1))//если в строке найден нужный символ - true, иначе false
                return true;
            return false;
        }

        //Метод возвращает true, если проверяемый символ - оператор
        private static bool IsOperator(char с)
        {
            if (("+-/*^()".IndexOf(с) != -1)) //если в строке найден нужный символ - true, иначе false
                return true;
            return false;
        }

        private static byte GetPriority(char s) //получение приоритета символов
        {
            switch (s)
            {
                case '(': return 0;
                case ')': return 1;
                case '+': return 2;
                case '-': return 3;
                case '*': return 4;
                case '/': return 4;
                case '^': return 5;
                default: return 6;
            }
        }

        //проверка на запрещенные символы
        private static bool DoesStringContainRestrictedSymbols(string input)
        {
            var restrictedSymbols = new[] {'?', '@', '\"', '&', '~', '`', ':', '%', '#', ';'};

            return restrictedSymbols.Any(symbol => input.Contains(symbol));//если есть = true, если нет = false
        }
    }
}