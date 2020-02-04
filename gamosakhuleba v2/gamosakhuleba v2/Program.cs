using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace gamosakhuleba_v2
{
    class Program
    {
        static void Main(string[] args)
        {
            string expression = Console.ReadLine();
            Console.WriteLine(MathExpresion.Calculation(expression));
            Console.ReadKey();
        }
    }

    static class MathExpresion
    {
        public static double Calculation(string expresstin)
        {
            double result = FillData(expresstin);
            return result;
        }

        private static double FillData(string expression)
        {
            expression.Replace(',', '.');
            char[] expressuInChars = expression.ToCharArray();
            Stack<double> values = new Stack<double>();
            Stack<char> operations = new Stack<char>();
            List<double> numbers = new List<double>();
            List<char> symbols = new List<char>();
            

            for (int i = 0; i < expressuInChars.Length; i++)
            {
                if (expressuInChars[i] == ' ')
                {
                    continue;
                }

                if (expressuInChars[i] >= '0' && expressuInChars[i] <= '9')
                {
                    StringBuilder number = new StringBuilder();
                    while (i < expressuInChars.Length && expressuInChars[i] >= '0' && expressuInChars[i] <= '9' || expressuInChars[i] == '.')
                    {
                        number.Append(expressuInChars[i]);
                        i++;
                        if (i == expressuInChars.Length)
                        {
                            break;
                        }
                    }
                    i--;
                    values.Push(double.Parse(number.ToString()));
                    if (charExsists(expressuInChars, ')', i-1) || charExsists(expressuInChars, '(', i + 1))
                    {
                        operations.Push('*');
                    }
                }
                else if (expressuInChars[i] == '(')
                {
                    if (operations.Count != 0 && operations.Peek() == ')')
                    {
                        operations.Pop();
                        operations.Push('*');
                    }
                    operations.Push(expressuInChars[i]);
                }
                else if (expressuInChars[i] == ')')
                {
                    if (values.Count !=1)
                    {
                        while (operations.Peek() != '(')
                        {
                            numbers.Add(values.Pop());
                            symbols.Add(operations.Pop());
                        }
                        numbers.Add(values.Pop());
                        values.Push(ResultWithMD(numbers, symbols));
                        numbers.Clear();
                        symbols.Clear();
                        operations.Pop();
                        if (charExsists(expressuInChars, '(',i+1)) 
                        {
                            operations.Push(expressuInChars[i]);
                        }
                    }else
                    {
                        operations.Push(expressuInChars[i]);
                        continue;
                    }
                    
                }
                else if (expressuInChars[i] == '+' || expressuInChars[i] == '-' || expressuInChars[i] == '*' || expressuInChars[i] == '/')
                {
                    if (expressuInChars[i] != '-')
                    {
                        operations.Push(expressuInChars[i]);
                    }
                    else if (operations.Count == 0 && expressuInChars[i] == '-' || expressuInChars[i] == '-' && operations.Peek() == '*'|| operations.Peek() == '/' || expressuInChars[i-1] == '(')
                    {
                        operations.Push('*');
                        values.Push(-1);
                    }
                    else {
                        operations.Push(expressuInChars[i]);
                    }
                }
            }
            while (values.Count > 1)
            {
                values.Push(Calculation(values.Pop(), values.Pop(), operations.Pop()));
            }
            return values.Pop();
        }
        private static bool charExsists(char[] charArray, char searchUnit, int index)
        {
           int i = index;

            if (i  < charArray.Length && charArray [i] == searchUnit)
            {
                return true;
            }
            return false;
        }

        private static double ResultWithMD(List<double> numbers, List<char> symbols)
        {
            int index = 0;
            double result = 0;

            if (symbols.Exists(item => item == '*') || symbols.Exists(item => item == '/'))
            {
                foreach (var item in symbols)
                {
                    if (item == '*' || item == '/')
                    {
                        index = symbols.IndexOf(item);
                        break;
                    }
                }
                result = Calculation(numbers[index + 1], numbers[index], symbols[index]);
                ResizeLists(numbers, symbols, index, result);
                ResultWithMD(numbers, symbols);
            }
            if (symbols.Count != 0)
            {
                ResultWithSumSubstract(numbers, symbols);
            }
            return numbers[0];
        }
        private static double ResultWithSumSubstract(List<double> numbers, List<char> symbols)
        {
            double result = Calculation(numbers[1], numbers[0], symbols[0]);
            ResizeLists(numbers, symbols, 0, result);

            if (numbers.Count > 1)
            {
                result = ResultWithSumSubstract(numbers, symbols);
            }
            return result;
        }
        static double Calculation(double x, double y, char action)
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
                    if (y == 0)
                    {
                        throw new Exception("Cannot divide by zero");
                    }
                    return x / y;
                default:
                    return 0;
            }
        }
        private static void ResizeLists(List<double> numbers, List<char> symbols, int index, double result)
        {
            numbers.RemoveRange(index, 2);
            numbers.Insert(index, result);
            symbols.RemoveAt(index);
        }
    }
}
