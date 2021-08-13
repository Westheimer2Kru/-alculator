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
            Console.WriteLine(Calculator.Calculate(input));
        }
    }

    class Calculator
    {
        static public float Calculate(string input) // Решение с помощью 2 стеков
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
                      if(Priority(operators.Peek()) < Priority(input[i])) // при большем приоритете просто помещаем оператор в стек
                        {
                            operators.Push(input[i]);
                        }
                      else // иначе вычисляем результат последних двух чисел и помещаем в первый стек
                        {
                            try
                            {
                                digits.Push(Compute(digits.Pop(), digits.Pop(), operators.Pop()));
                                i--;
                            } catch (Exception e)
                            {
                                Console.WriteLine("Ошибка! Возможно вы допустили опечатку!");
                                Console.WriteLine(e);
                                return 0;
                            }
                            
                             // шаг назад, чтобы вернуться к символу
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
                            digits.Push(Compute(digits.Pop(), digits.Pop(), operators.Pop()));
                            if(operators.Count == 0)
                            {
                                Console.WriteLine("Пропущена открывающая скобка");
                                return 0;
                            }
                        }
                        operators.Pop();
                            
                    }
                }
            }

            try
            {
                while (operators.Count > 0) // если по окончанию строки остались действия, вычисляем их
                    digits.Push(Compute(digits.Pop(), digits.Pop(), operators.Pop()));
            }catch(Exception e)
            {
                Console.WriteLine("Произошла ошибка! Возможно, вы упустили скобку");
                Console.WriteLine(e);
                return 0;
            }
            solve = digits.Pop(); // в стеке должно остаться одно число - наш ответ
            return solve;
        }

        static private float Compute(float num1, float num2, char oper)
        {
            if(oper == '(')
            {
                Console.WriteLine("Пропущена закрывающая скобка!");
                return 0;
            }
            if (oper == '+')
                return num1 + num2;
            else if (oper == '-')
                return num2 - num1;
            else if (oper == '*')
                return num1 * num2;
            else
                return num2 / num1;
        }

        static private bool IsOperator(char oper)
        {
            if ("+-*/".Contains(oper))
                return true;
            return false;
        }
        static private bool IsBracket(char oper)
        {
            if ("()".Contains(oper))
                return true;
            return false;
        }
        static private int Priority(char oper)
        {
            return oper switch
            {
                '+' => 1,
                '-' => 1,
                '*' => 2,
                '/' => 2,
                _ => 0,
            };
        }

    }
}
