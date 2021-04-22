//using System;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;

//namespace LabsCompilator
//{
//    class Program
//    {
//        static Regex regexLetter = new Regex(@"^[\p{L}]+$");
//        static Regex regexLetterEng = new Regex(@"^[a-zA-Z]+$");
//        static Regex regexNumberFirst = new Regex(@"^[\p{N}]+$");
//        static Regex regexNumber = new Regex(@"^[\p{N}]+$");
//        static Regex regexNumberOrLetter = new Regex(@"^[\p{L}\p{N}]+$");
//        public static string Identificator(string context, int index, int max)
//        {
//            if (context.Length == index + 1)
//            {
//                if (context.Length == 0)
//                {
//                    return "Введите что-нибудь";
//                }
//                if (context.Length >= 1)
//                {
//                    MatchCollection matches = regexLetter.Matches(context[0].ToString());
//                    if (matches.Count > 0)
//                    {
//                        return "Выражение является идентификатором";
//                    }
//                    return "Идентификатор начинается с буквы!";

//                }
//                return "Выражение является идентификатором";
//            }
//            index++;
//            MatchCollection matchesTwo = regexNumberOrLetter.Matches(context[index].ToString());
//            if (matchesTwo.Count > 0)
//            {
//                return Identificator(context, index, max);
//            }
//            return "Произошла ошибка: index = " + (index + 1) + " symbol = " + context[index];
//        }

//        public static string Number(string context, int index, int max)
//        {
//            if (context.Length == index + 1)
//            {
//                if (context.Length == 0)
//                {
//                    return "Введите что-нибудь";
//                }
//                if (context.Length >= 1)
//                {
//                    MatchCollection matches = regexNumber.Matches(context[0].ToString());
//                    if (matches.Count > 0)
//                    {
//                        return "Выражение является целым числом";
//                    }
//                    return "Целое число должно начинаться с цифр от 1 до 9!";

//                }
//                return "Выражение является идентификатором";
//            }
//            if (index == 0)
//            {
//                MatchCollection matches = regexNumber.Matches(context[0].ToString());
//                if (matches.Count < 0)
//                {
//                    return "Целое число должно начинаться с цифр от 1 до 9!";
//                }
//            }
//            index++;
//            MatchCollection matchesTwo = regexNumber.Matches(context[index].ToString());
//            if (matchesTwo.Count > 0)
//            {
//                return Number(context, index, max);
//            }
//            else
//            {
//                return "Произошла ошибка: index = " + (index + 1) + " symbol = " + context[index];
//            }
//        }


//        public static void CheckInteger(string val)
//        {
//            Process process = new Process();
//            int index = 0;
//            for (int i = 0; i < val.Length; i++)
//            {
//                if (process.CurrentState != ProcessState.Error)
//                {
//                    MatchCollection matches = regexNumber.Matches(val[i].ToString());
//                    if (matches.Count == 0)
//                    {
//                        process.MoveNext(Command.Exception);
//                        index = i + 1;
//                        continue;
//                    }
//                    if (i == val.Length - 1 && process.CurrentState != ProcessState.Terminated)
//                    {
//                        process.MoveNext(Command.Exit);
//                        continue;
//                    }
//                    process.MoveNext(Command.Resume);
//                }
//            }

//            if (process.CurrentState == ProcessState.Terminated)
//            {
//                Console.WriteLine("Выражение является целым числом, его состояние: " + process.ToString());
//            }
//            if (index != 0)
//            {
//                Console.WriteLine("Ошибка в индексе под номером " + index);
//            }
//        }

//        public static string CheckIdentifier(string value)
//        {
//            ProcessIdentifier process = new ProcessIdentifier();
//            for (int i = 0; i < value.Length; i++)
//            {
//                if (process.CurrentState != ProcessStateIdentifier.Error)
//                {
//                    if (process.CurrentState == ProcessStateIdentifier.Inactive)
//                    {

//                        MatchCollection matchesLetter = regexLetter.Matches(value[i].ToString());
//                        if (matchesLetter.Count == 0)
//                        {
//                            process.MoveNext(CommandIdentifier.Exception);
//                            return "Ошибка в индексе: " + (i + 1) + " состояние: " + process.ToString();
//                        }
//                        process.MoveNext(CommandIdentifier.Begin);
//                    }
//                    else
//                    {
//                        MatchCollection matchesNumber = regexNumber.Matches(value[i].ToString());
//                        if (matchesNumber.Count == 0)
//                        {
//                            MatchCollection matchesLetter = regexLetter.Matches(value[i].ToString());
//                            if (matchesLetter.Count == 0)
//                            {
//                                process.MoveNext(CommandIdentifier.Exception);
//                                return "Ошибка в индексе: " + (i + 1) + " состояние: " + process.ToString();
//                            }
//                            process.MoveNext(CommandIdentifier.ResumeLetter);
//                        }
//                        process.MoveNext(CommandIdentifier.ResumeNumeral);
//                    }
//                }
//                if (i == value.Length - 1)
//                {
//                    if (process.CurrentState != ProcessStateIdentifier.Error)
//                    {
//                        process.MoveNext(CommandIdentifier.Exit);
//                        return "Данное выражение является идентификатором";
//                    }
//                }
//            }
//            return "Результат = " + process.ToString();
//        }

//        public static void CheckArithmetic(string val)
//        {
//            ProcessAriphmetic process = new ProcessAriphmetic();
//            Console.WriteLine("InitState: " + process.CurrentState.ToString());
//            int index = 0;
//            for (int i = 0; i < val.Length; i++)
//            {
//                if (process.CurrentState != ProcessStateAriphmetic.Error)
//                {
//                    MatchCollection matches = regexNumber.Matches(val[i].ToString());
//                    if (matches.Count == 0)
//                    {
//                        if (process.CurrentState != ProcessStateAriphmetic.FirstPhase && process.CurrentState != ProcessStateAriphmetic.SecondPhase && process.CurrentState != ProcessStateAriphmetic.ThirdPhase & process.CurrentState != ProcessStateAriphmetic.FourPhase)
//                        {
//                            process.MoveNext(CommandAriphmetic.Exception);
//                            index = i + 1;
//                            continue;
//                        }
//                        if (val[i] == '/' || val[i] == '*' || val[i] == '-' || val[i] == '+')
//                        {
//                            if (process.CurrentState == ProcessStateAriphmetic.Aripthmetic)
//                            {
//                                process.MoveNext(CommandAriphmetic.Exception);
//                                index = i + 1;
//                                continue;
//                            }
//                            process.MoveNext(CommandAriphmetic.AripthmenticResume);
//                        }
//                    }
//                    else
//                    {
//                        //if (i == val.Length - 1 && process.CurrentState != ProcessStateAriphmetic.Terminated)
//                        //{
//                        //    process.MoveNext(CommandAriphmetic.Exit);
//                        //    continue;
//                        //}
//                        process.MoveNext(CommandAriphmetic.Resume);
//                    }//if (process.CurrentState == ProcessStateAriphmetic.FirstPhase || process.CurrentState == ProcessStateAriphmetic.SecondPhase)
//                    //{
//                    //    MatchCollection matches = regexNumber.Matches(val[i].ToString());
//                    //    if (matches.Count == 0)
//                    //    {
//                    //        process.MoveNext(CommandAriphmetic.Exception);
//                    //        index = i + 1;
//                    //        continue;
//                    //    }
//                    //    if (i == val.Length - 1 && process.CurrentState != ProcessStateAriphmetic.Terminated)
//                    //    {
//                    //        process.MoveNext(CommandAriphmetic.Exit);
//                    //        continue;
//                    //    }
//                    //    process.MoveNext(CommandAriphmetic.Resume);
//                    //}

//                }
//            }
//            if (process.CurrentState == ProcessStateAriphmetic.ThirdPhase || process.CurrentState == ProcessStateAriphmetic.FourPhase)
//            {
//                process.MoveNext(CommandAriphmetic.Exit);
//            }
//            if (process.CurrentState == ProcessStateAriphmetic.Terminated)
//            {
//                Console.WriteLine("Данное выражение является арифметическим: Конечное состояние " + process.ToString());
//            }
//            if (index != 0)
//            {
//                Console.WriteLine("Ошибка в индексе под номером " + index);
//            }
//        }


//        static void Main(string[] args)
//        {
//            Console.WriteLine("Введите выражение: ");
//            string val = "";
//            val = Console.ReadLine();
//            //var result = Identificator(val, 0, val.Length);
//            //var result = Number(val, 0, val.Length);
//            //CheckInteger(val);
//            //string result = CheckIdentifier(val);
//            CheckArithmetic(val);
//            //BinaryTree<int> integerTree = new BinaryTree<int>();

//            //Random rand = new Random();

//            //for (int i = 0; i < 20; i++)
//            //{
//            //    int value = rand.Next(100);
//            //    Console.WriteLine("Adding {0}", value);
//            //    integerTree.Add(value);
//            //}


//        }



//        public class Arifmethic
//        {
//            public char symbol { get; set; }
//            public NumberInt firstElem { get; set; }
//            public NumberInt secondElem { get; set; }

//        }

//        public class NumberInt
//        {
//            public int value { get; set; }
//            public Arifmethic head { get; set; }

//        }

//        public bool checkInt(string symbol)
//        {
//            MatchCollection matches = regexNumber.Matches(symbol);
//            if (matches.Count > 1)
//            {
//                return true;
//            }
//            return false;
//        }

//        public bool arifmethic(char symbol)
//        {
//            switch (symbol)
//            {
//                case '/': return true;
//                case '*': return true;
//                case '+': return true;
//                case '-': return true;
//                default: return false;
//            }
//        }

//        public string ResultArifmethic(string value)
//        {
//            if (value.Length == 0)
//                return null;
//            int startIndex = 0;
//            int endIndex = 0;
//            Elements element = new Elements();
//            element.first = true;
//            for (int i = 0; i <= value.Length; i++)
//            {
//                if (checkInt(value[i].ToString()))
//                {
//                    endIndex++;
//                }
//                else
//                {
//                    if (startIndex != endIndex)
//                    {
//                        if (NumberCheck(value, startIndex, endIndex))
//                        {
//                            if (element.first == true)
//                            {
//                                element.values = value.Substring(startIndex, endIndex - startIndex);
//                                element.character = Character.integer;
//                            }
//                            else
//                            {
//                                Elements temp = new Elements();
//                                temp.values = value.Substring(startIndex, endIndex - startIndex);
//                                temp.character = Character.integer;
//                                temp.beforeElement = element;
//                            }
//                        }
//                    }
//                }
//            }
//            return null;
//        }

//        public class Elements
//        {
//            public bool first { get; set; } = false;
//            public string values { get; set; }
//            public Character character { get; set; }
//            public Elements afterElement { get; set; }
//            public Elements beforeElement { get; set; }

//            /*public Elements NextElement()
//            {
//                if (beforeElement == null)
//                {
//                    return beforeElement
//                }
//            }*/
//        }

//        public enum Character
//        {
//            integer,
//            arithmetic,
//            param,
//            typeParam,
//            undefined
//        }

//        public class Rule
//        {
//            public Character character { get; set; }
//            public List<Rule> rules { get; set; }
//        }

//        public static bool NumberCheck(string context, int index, int max)
//        {
//            if (max == index + 1)
//            {
//                if (max == 0)
//                {
//                    return false;
//                }
//                if (max >= 1)
//                {
//                    MatchCollection matches = regexNumber.Matches(context[0].ToString());
//                    if (matches.Count > 0)
//                    {
//                        return true;
//                    }
//                    return false;

//                }
//                return true;
//            }
//            if (index == 0)
//            {
//                MatchCollection matches = regexNumber.Matches(context[0].ToString());
//                if (matches.Count < 0)
//                {
//                    return false;
//                }
//            }
//            index++;
//            MatchCollection matchesTwo = regexNumber.Matches(context[index].ToString());
//            if (matchesTwo.Count > 0)
//            {
//                return NumberCheck(context, index, max);
//            }
//            else
//            {
//                Console.WriteLine("Произошла ошибка: index = " + (index + 1) + " symbol = " + context[index]);
//                return false;
//            }
//        }
//    }
//}


using System;
using System.Collections.Generic;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TreeNode tree = new TreeNode();
            Random rand = new Random();
            List<int> array = new List<int>();
            Console.WriteLine("Массив: ");
            string arrayString = "";
            int number;
            for (int i = 0; i < 10; i++)
            {
                do
                {
                    number = rand.Next(1, 11);
                } while (array.Contains(number));
                array.Add(number);
                arrayString += " " + number.ToString();
            }

            array = Sorrt(array);
            Console.WriteLine(arrayString);
            //Console.WriteLine("Массив: ");
            //arrayString = "";
            //for (int i = 0; i < 20; i++)
            //{
            //    arrayString += " " + array[i].ToString();
            //}

            //Console.WriteLine(arrayString);
            Console.WriteLine("Дерево: ");
            //foreach (var temp in array)
            //{
            //    tree.Root = tree.AddNode(temp, tree.Root);
            //}
            int index = array.Count / 2 - 1;
            for (int i = 0; i < array.Count / 2; i++)
            {
                if (i == 0)
                {
                    tree.Root = tree.AddNode(array[index], tree.Root);
                }
                else
                {
                    tree.Root = tree.AddNode(array[i - 1], tree.Root);
                    tree.Root = tree.AddNode(array[i + 1], tree.Root);
                }
            }
            tree.PrintTree(Console.WindowWidth / 2, 5, tree.Root);
            Console.SetCursorPosition(0, 25);
            Console.ReadKey();

        }

        public static List<int> Sorrt(List<int> arr)
        {
            int temp = 0;
            for (int write = 0; write < arr.Count; write++)
            {
                for (int sort = 0; sort < arr.Count - 1; sort++)
                {
                    if (arr[sort] < arr[sort + 1])
                    {
                        temp = arr[sort + 1];
                        arr[sort + 1] = arr[sort];
                        arr[sort] = temp;
                    }
                }
            }
            return arr;
        }
    }


    public class TreeNode
    {
        private Node _root;

        public Node Root { get => _root; set => _root = value; }

        public Node AddNode(int inputDataNode, Node root)
        {
            if (root == null)
            {
                root = new Node(inputDataNode);
            }
            else
            {
                if (inputDataNode < root.Data)
                {
                    root.Left = AddNode(inputDataNode, root.Left);
                }
                else
                {
                    root.Right = AddNode(inputDataNode, root.Right);
                }
            }

            return root;
        }

        public Node FindElement(int findData, Node root)
        {
            if (root == null || findData == root.Data)
                return root;
            else if (root.Data < findData)
                return FindElement(findData, root.Left);
            else
                return FindElement(findData, root.Right);
        }

        public Node Minimum(Node root)
        {
            if (root != null)
            {
                if (root.Left != null) root = Minimum(root.Left);
            }
            return root;
        }

        public Node DeleteNode(int deleteData, Node root)
        {
            if (root == null)
                return root;
            if (deleteData < root.Data)
            {
                root.Left = DeleteNode(deleteData, root.Left);
            }
            else if (deleteData > root.Data)
            {
                root.Right = DeleteNode(deleteData, root.Right);
            }
            else if (root.Left != null && root.Right != null)
            {
                root.Data = Minimum(root.Right).Data;
                root.Right = DeleteNode(root.Data, root.Right);
            }
            else if (root.Left != null)
            {
                return root.Left;
            }
            else
            {
                return root.Right;
            }

            return root;

        }

        public void PrintTree(int x, int y, Node root, int delta = 0)
        {
            if (root != null)
            {
                if (delta == 0) delta = x / 2;
                Console.SetCursorPosition(x, y);
                Console.Write(root.Data);
                PrintTree(x - delta, y + 3, root.Left, delta / 2);
                PrintTree(x + delta, y + 3, root.Right, delta / 2);
            }

        }

        public void ClearTree()
        {
            _root = null;
        }

        public int CountElements(Node root)
        {
            if (root == null)
                return 0;
            else
            {
                int count = 0;
                count += CountElements(root.Left);
                count += CountElements(root.Right);

                return count + 1;
            }
        }

        public int SummaElements(Node root)
        {
            if (root == null)
                return 0;
            else
            {
                int count = 0;
                count += SummaElements(root.Left);
                count += SummaElements(root.Right);

                return count + root.Data;
            }
        }

        public bool IsEmpty()
        {
            return _root == null ? true : false;
        }

    }

    public class Node
    {
        private int _data;
        private Node _left;
        private Node _right;

        public Node()
        {
        }

        public Node(int inputDataNode)
        {
            Data = inputDataNode;
        }

        public Node(int data, Node left, Node right)
        {
            Data = data;
            Left = left;
            Right = right;
        }

        public int Data { get => _data; set => _data = value; }
        public Node Left { get => _left; set => _left = value; }
        public Test.Node Right { get => _right; set => _right = value; }
    }
}