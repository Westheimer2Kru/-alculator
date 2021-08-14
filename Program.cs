using System;
using System.Collections.Generic;


// Учтены операторы + - * / ( ) 
namespace Сalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ваше выражение:");
            string input = Console.ReadLine();
            Calculator calc = new Calculator();
            Console.WriteLine(calc.Calculate(input));
        }
    }

    interface IOperation
    {
        public int Priority { get; }
        public float CalculateResult(float first, float second);
    }

    class Addition : IOperation
    {
        private static int priority = 1;
        public int Priority
            {
                get => priority;
            }
        public float CalculateResult(float first, float second)
        {
            return first + second;
        }
    }

    class Subtraction : IOperation
    {
        private static int priority = 1;
        public int Priority
        {
            get => priority;
        }
        public float CalculateResult(float first, float second)
        {
            return second - first;
        }
    }

    class Multiplication: IOperation
    {
        private static int priority = 2;
        public int Priority
        {
            get => priority;
        }
        public float CalculateResult(float first, float second)
        {
            return first * second;
        }
    }

    class Division: IOperation
    {
        private static int priority = 2;
        public int Priority
        {
            get => priority;
        }
        public float CalculateResult(float first, float second)
        {
            return second / first;
        }

    }

    class Calculator
    {
        private Dictionary<char,IOperation> operatorTable = new Dictionary<char, IOperation>();

        public Calculator()
        {
            operatorTable.Add('+', new Addition());
            operatorTable.Add('-', new Subtraction());
            operatorTable.Add('*', new Multiplication());
            operatorTable.Add('/', new Division());
        }
        public float Calculate(string input) // Решение с помощью 2 стеков
        {
            float solve;
            input = input.Trim(' ');
            Stack<float> digits = new Stack<float>();
            Stack<char> operators = new Stack<char>();

            for(int i=0; i<input.Length; i++)
            {
                if (Char.IsDigit(input[i])) // если символ число, добавляем в первый стек
                {
                    digits.Push((float)Char.GetNumericValue(input[i]));
                }               
                else if(IsOperator(input[i]))
                {
                    if (operators.Count == 0) // в пустой стек просто добавляем оператор
                        operators.Push(input[i]);
                    else
                    {
                        try
                        {
                            if (operatorTable[operators.Peek()].Priority < operatorTable[input[i]].Priority) // при большем приоритете просто помещаем оператор в стек
                            {
                                operators.Push(input[i]);
                            }
                            else // иначе вычисляем результат последних двух чисел и помещаем в первый стек
                            {
                                digits.Push(operatorTable[operators.Pop()].CalculateResult(digits.Pop(), digits.Pop()));
                                i--;// шаг назад, чтобы вернуться к символу
                            }
                        }catch(KeyNotFoundException)
                        {
                            operators.Push(input[i]);
                        }
                        
                    }
                } else if (IsBracket(input[i])) // открывающую скобку добавляем во второй стек, закрывающая же вычисляет выражение в скобках
                {
                    if (input[i] == '(')
                        operators.Push(input[i]);
                    else
                    {
                        if (operators.Count == 0)
                        {
                            Console.WriteLine("Пропущена открывающая скобка");
                            return 0;
                        }
                        while (operators.Peek() != '(')
                        {
                            digits.Push(operatorTable[operators.Pop()].CalculateResult(digits.Pop(), digits.Pop()));
                            if (operators.Count == 0)
                            {
                                Console.WriteLine("Пропущена открывающая скобка");
                                return 0;
                            }
                        }
                        operators.Pop();
                            
                    }
                }
            }
            while (operators.Count > 0) // если по окончанию строки остались действия, вычисляем их
                  digits.Push(operatorTable[operators.Pop()].CalculateResult(digits.Pop(), digits.Pop()));

            solve = digits.Pop(); // в стеке должно остаться одно число - наш ответ
            return solve;
        }
        static private bool IsOperator(char oper)
        {
            return "+-*/".Contains(oper);
        }
        static private bool IsBracket(char oper)
        {
            return "()".Contains(oper);
        }
    }
}