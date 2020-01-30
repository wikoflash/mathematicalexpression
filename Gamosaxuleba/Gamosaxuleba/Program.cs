using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Gamosaxuleba
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                string expression = Console.ReadLine();
                MyTools myTools = new MyTools(expression);
            } while (true);
        }
    }

    class MyTools
    {
        public MyTools(string expession)
        {
            if (!CheckExpression(expession))
            {
                throw new Exception("Mathematical experssion cannot accept alphabetical expersions");
            }
            FillData(expession, numbers, symbols);
            var result = Result(numbers, symbols);
            Console.WriteLine(result);
        }
        List<double> numbers = new List<double>();
        List<char> symbols = new List<char>();

        private double Result(List<double> numbers, List<char> symbols)
        {
            ResultWithM(numbers, symbols);
            return numbers[0];
        }
        private void ResultWithM(List<double> numbers, List<char> symbols)
        {
            int x = symbols.FindIndex(item => item == '*');
            double result;
            if (symbols.Exists(item => item == '*'))
            {
                if (x == 0)
                {
                    result = Calculation(numbers[0], numbers[1], symbols[0]);
                    ResizeLists(numbers, symbols, x, result);
                }
                else
                {
                    result = Calculation(numbers[x], numbers[x + 1], symbols[x]);
                    ResizeLists(numbers, symbols, x, result);
                }
                ResultWithM(numbers, symbols);
            }
            ResultWithD(numbers, symbols);
        }
        private void ResultWithD(List<double> numbers, List<char> symbols)
        {
            int x = symbols.FindIndex(item => item == '/');
            double result;
            if (symbols.Exists(item => item == '/'))
            {
                if (x == 0)
                {
                    result = Calculation(numbers[0], numbers[1], symbols[0]);
                    ResizeLists(numbers, symbols, x, result);
                }
                else
                {
                    result = Calculation(numbers[x], numbers[x + 1], symbols[x]);
                    ResizeLists(numbers, symbols, x, result);
                }
                ResultWithD(numbers, symbols);
            }
            if (symbols.Count != 0)
            {
                ResultWithSumsubstract(numbers, symbols);
            }
        }
        private double ResultWithSumsubstract(List<double> numbers, List<char> symbols)
        {
            double result = Calculation(numbers[0], numbers[1], symbols[0]);
            ResizeLists(numbers, symbols, 0, result);

            if (numbers.Count > 1)
            {
                result = ResultWithSumsubstract(numbers, symbols);
            }
            return result;
        }
        private void ResizeLists(List<double> numbers, List<char> symbols, int index, double result)
        {
            numbers.RemoveRange(index, 2);
            numbers.Insert(index, result);
            symbols.RemoveAt(index);
        }
        private double Calculation(double x, double y, char action)
        {
            switch (action)
            {
                case '+':
                    return x + y;
                case '-':
                    return x - y;
                case '*':
                    return x * y;
                case '/':
                    return x / y;
                default:
                    return 0;
            }
        }
        private void FillData(string expression, List<double> numbers, List<char> symbols)
        {
            char[] mainOperations= { '*', '-', '+', '/' };
            string[] items= expression.Split(mainOperations);

            foreach (var item in items)
            {
                numbers.Add(Convert.ToDouble(item));
            }

            foreach (var symb in expression)
            {
                if (symb=='*' || symb == '/'||  symb =='+' || symb== '-'  )
                {
                    symbols.Add(symb);
                }
            }
        }
        private bool CheckExpression(string x)
        {
            foreach (var item in x)
            {
                if (item < '(' || item > '9')
                {
                    return false;
                }
            }
            return true;
        }
    }
}
