using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

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
                double res = myTools.Result();
                Console.WriteLine(res);

            } while (true);
        }
    }

    class MyTools
    {
        public MyTools(string expression)
        {
            Expession = expression;
        }

        private string _expession;
        List<double> numbers = new List<double>();
        List<char> symbols = new List<char>();

        private string Expession
        {
            get
            {
                return _expession;
            }
            set
            {
                if (!CheckExpression(value))
                {
                    throw new Exception("Mathematical experssion cannot accept alphabetical expersions");
                }
                else
                {
                    _expession = value;
                }
            }
        }

        public double Result()
        {
            if (CheckBrackets(Expession))
            {
                FillDataWithBrackets(Expession, numbers, symbols, Expession);
            }
            else
            {
                FillData(Expession, numbers, symbols);
            }
            ResultWithMD(numbers, symbols);
            return numbers[0];
        }
        
        private void ResultWithMD(List<double> numbers, List<char> symbols)
        {
            int index = 0;
            double result;

            if (symbols.Exists(item => item == '*') || symbols.Exists(item => item == '/'))
            {
                foreach (var item in symbols)
                {
                    if(item == '*' || item == '/')
                    {
                        index = symbols.IndexOf(item);
                        break;
                    }
                }
                result = Calculation(numbers[index], numbers[index + 1], symbols[index]);
                ResizeLists(numbers, symbols, index, result);
                ResultWithMD(numbers, symbols);
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
        private void FillDataWithBrackets(string expression, List<double> numbers, List<char> symbols, string start, int pastbracketLvl = 0)
        {
            string midString = "";
            string startstring;
            string temp;
            bool isBracket = false;
            int bracketLvl = 0;
            int startIndex = 0; 
            int endIndex = expression.Length - 1;
            foreach (var item in expression)
            {
                if (item == '(')
                {
                    isBracket = true;
                    bracketLvl++;
                    if (bracketLvl> 1)
                    {
                        startIndex++;
                        FillDataWithBrackets(expression.Substring(startIndex, expression.Length - startIndex), numbers, symbols, midString, bracketLvl);
                        //if(numbers.Count > 1)
                        //{
                        //    ResultWithMD(numbers, symbols);
                        //    temp = numbers[0].ToString();
                        //    numbers.Clear();
                        //    symbols.Clear();
                        //    break;
                        //}
                    }
                    continue;
                }
                else if (item == ')' /*|| bracketLvl >1 */)
                {
                    isBracket = false;
                    endIndex = expression.IndexOf(item) + 1;
                    bracketLvl--;
                    break;
                }
                while (isBracket)
                {
                    midString += item.ToString();
                    break;
                }
                startIndex++;
                if (checkForData(numbers))
                {
                    return;
                }
            }
            if (bracketLvl == 0)
            {
                startstring = expression.Substring(0, expression.IndexOf('('));
            }
            else
            {
                startstring = expression.Substring(0, expression.LastIndexOf('('));
            }
            string endtstring = expression.Substring(endIndex, expression.Length - endIndex);
            FillData(midString, numbers, symbols);
            ResultWithMD(numbers, symbols);
            double result = numbers[0];

            if (pastbracketLvl > 1)
            {
                midString = start + startstring + result.ToString() + endtstring;
            }
            else 
            {
                midString = startstring + result.ToString() + endtstring;
            }

            numbers.Clear();
            symbols.Clear();

            if (CheckBrackets(midString))
            {
                FillDataWithBrackets(midString, numbers, symbols, startstring);
            }
            if (!string.IsNullOrEmpty(midString) && CheckBrackets(midString) == false)
            {
                if (!checkForData(numbers))
                {
                    if (midString[midString.Length - 1] == ')')
                    {
                        midString = midString.Substring(0, midString.Length - 1);
                    }
                    FillData(midString, numbers, symbols);
                }
                else
                {
                    return;
                }
            }
        }

        private void FillData(string expression, List<double> numbers, List<char> symbols)
        {
            char[] mainOperations = { '*', '-', '+', '/', ')'};
            string[] items = expression.Split(mainOperations);
            List<int> index = new List<int>();

            foreach (var item in items)
            {
                if (item == "")
                {
                    numbers.Add(-1);
                    index.Add(numbers.LastIndexOf(-1));
                }
                else if(item == ")")
                {
                    continue;
                }
                else { numbers.Add(Convert.ToDouble(item)); }

            }
            foreach (var symb in expression)
            {
                if (symb=='*' || symb == '/'||  symb =='+' || symb== '-'  )
                {
                    symbols.Add(symb);
                }
            }

            foreach (var item in index)
            {
                if (symbols.Count > item)
                {
                    symbols.RemoveAt(item);
                    symbols.Insert(item, '*');
                }
                else { symbols.Add('*'); }
            }
        }

        //private string SearchInsideBrackets(string expression, List<double> numbers, List<char> symbols)
        //{
        //    int firstIndex = expression.IndexOf('(') + 1;
        //    int lastIndex = expression.LastIndexOf(')');
        //    string startstring = expression.Substring(0, expression.IndexOf('('));
        //    string endtstring = expression.Substring(lastIndex, expression.Length - lastIndex);
        //    string midString = expression.Substring(firstIndex, lastIndex - firstIndex);
        //    if (midString.IndexOf('(') != -1)
        //    {
        //        SearchInsideBrackets(midString, numbers, symbols);
        //    }

        //    //FillDataWithIndex(midString, numbers, symbols);
        //    return midString;
        //}

        //private void FillDataWithIndex(string expression, List<double> numbers, List<char> symbols)
        //{
        //    char[] mainOperations = { '*', '-', '+', '/' };
        //    string[] items = expression.Split(mainOperations);
        //    int index;

        //    foreach (var item in items)
        //    {
        //        if (item == "")
        //        {
        //        }
        //        else { numbers.Insert(0, Convert.ToDouble(item)); }
        //    }
        //    foreach (var symb in expression)
        //    {
        //        if (symb == '*' || symb == '/' || symb == '+' || symb == '-')
        //        {
        //            symbols.Add(symb);
        //        }
        //    }
        //}

        private bool checkForData(List<double> numbers)
        {
            if (numbers.Count > 0)
            {
                return true;
            }
            return false;
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
        private bool CheckBrackets(string expression)
        {
            if (expression.IndexOf('(') != -1)
            {
                return true;
            }
            return false;
        }
        private bool CheckExpression(string expression)
        {
            foreach (var item in expression)
            {
                if (item < '(' || item > '9')
                {
                    return false;
                }
            }
            return true;
        }
        private void ResizeLists(List<double> numbers, List<char> symbols, int index, double result)
        {
            numbers.RemoveRange(index, 2);
            numbers.Insert(index, result);
            symbols.RemoveAt(index);
        }
    }
}
