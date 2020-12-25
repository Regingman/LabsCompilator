using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LabsCompilator
{
    class Program
    {
        static Regex regexLetter = new Regex(@"^[\p{L}]+$");
        static Regex regexNumberFirst = new Regex(@"^[\p{N}]+$");
        static Regex regexNumber = new Regex(@"^[\p{N}]+$");
        static Regex regexNumberOrLetter = new Regex(@"^[\p{L}\p{N}]+$");
        public static string Identificator(string context, int index, int max)
        {
            if (context.Length == index + 1)
            {
                if (context.Length == 0)
                {
                    return "Введите что-нибудь";
                }
                if (context.Length >= 1)
                {
                    MatchCollection matches = regexLetter.Matches(context[0].ToString());
                    if (matches.Count > 0)
                    {
                        return "Выражение является идентификатором";
                    }
                    return "Идентификатор начинается с буквы!";

                }
                return "Выражение является идентификатором";
            }
            index++;
            MatchCollection matchesTwo = regexNumberOrLetter.Matches(context[index].ToString());
            if (matchesTwo.Count > 0)
            {
                return Identificator(context, index, max);
            }
            return "Произошла ошибка: index = " + (index + 1) + " symbol = " + context[index];
        }

        public static string Number(string context, int index, int max)
        {
            if (context.Length == index + 1)
            {
                if (context.Length == 0)
                {
                    return "Введите что-нибудь";
                }
                if (context.Length >= 1)
                {
                    MatchCollection matches = regexNumber.Matches(context[0].ToString());
                    if (matches.Count > 0)
                    {
                        return "Выражение является целым числом";
                    }
                    return "Целое число должно начинаться с цифр от 1 до 9!";

                }
                return "Выражение является идентификатором";
            }
            if (index == 0)
            {
                MatchCollection matches = regexNumber.Matches(context[0].ToString());
                if (matches.Count < 0)
                {
                    return "Целое число должно начинаться с цифр от 1 до 9!";
                }
            }
            index++;
            MatchCollection matchesTwo = regexNumber.Matches(context[index].ToString());
            if (matchesTwo.Count > 0)
            {
                return Number(context, index, max);
            }
            else
            {
                return "Произошла ошибка: index = " + (index + 1) + " symbol = " + context[index];
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите выражение: ");
            string val = "";
            val = Console.ReadLine();
            //var result = Identificator(val, 0, val.Length);
            // var result = Number(val, 0, val.Length);
            Process process = new Process();
            int index = 0;
            for (int i = 0; i < val.Length; i++)
            {
                if (process.CurrentState != ProcessState.Error)
                {
                    MatchCollection matches = regexNumber.Matches(val[i].ToString());
                    if (matches.Count == 0)
                    {
                        process.MoveNext(Command.Exception);
                        index = i + 1;
                        continue;
                    }
                    if (i == val.Length - 1 && process.CurrentState != ProcessState.Terminated)
                    {
                        process.MoveNext(Command.Exit);
                        continue;
                    }
                    process.MoveNext(Command.Resume);
                }
            }
            
            if (process.CurrentState == ProcessState.Terminated)
            {
                Console.WriteLine("Результат: " + process.ToString());
            }
            if(index != 0)
            {
                Console.WriteLine("Ошибка в индексе под номером " + index);
            }

        }

        public class Arifmethic
        {
            public char symbol { get; set; }
            public NumberInt firstElem { get; set; }
            public NumberInt secondElem { get; set; }

        }

        public class NumberInt
        {
            public int value { get; set; }
            public Arifmethic head { get; set; }

        }

        public bool checkInt(string symbol)
        {
            MatchCollection matches = regexNumber.Matches(symbol);
            if (matches.Count > 1)
            {
                return true;
            }
            return false;
        }

        public bool arifmethic(char symbol)
        {
            switch (symbol)
            {
                case '/': return true;
                case '*': return true;
                case '+': return true;
                case '-': return true;
                default: return false;
            }
        }

        public string ResultArifmethic(string value)
        {
            if (value.Length == 0)
                return null;
            int startIndex = 0;
            int endIndex = 0;
            Elements element = new Elements();
            element.first = true;
            for (int i = 0; i <= value.Length; i++)
            {
                if (checkInt(value[i].ToString()))
                {
                    endIndex++;
                }
                else
                {
                    if (startIndex != endIndex)
                    {
                        if (NumberCheck(value, startIndex, endIndex))
                        {
                            if (element.first == true)
                            {
                                element.values = value.Substring(startIndex, endIndex - startIndex);
                                element.character = Character.integer;
                            }
                            else
                            {
                                Elements temp = new Elements();
                                temp.values = value.Substring(startIndex, endIndex - startIndex);
                                temp.character = Character.integer;
                                temp.beforeElement = element;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public class Elements
        {
            public bool first { get; set; } = false;
            public string values { get; set; }
            public Character character { get; set; }
            public Elements afterElement { get; set; }
            public Elements beforeElement { get; set; }

            /*public Elements NextElement()
            {
                if (beforeElement == null)
                {
                    return beforeElement
                }
            }*/
        }

        public enum Character
        {
            integer,
            arithmetic,
            param,
            typeParam,
            undefined
        }

        public class Rule
        {
            public Character character { get; set; }
            public List<Rule> rules { get; set; }
        }

        public static bool NumberCheck(string context, int index, int max)
        {
            if (max == index + 1)
            {
                if (max == 0)
                {
                    return false;
                }
                if (max >= 1)
                {
                    MatchCollection matches = regexNumber.Matches(context[0].ToString());
                    if (matches.Count > 0)
                    {
                        return true;
                    }
                    return false;

                }
                return true;
            }
            if (index == 0)
            {
                MatchCollection matches = regexNumber.Matches(context[0].ToString());
                if (matches.Count < 0)
                {
                    return false;
                }
            }
            index++;
            MatchCollection matchesTwo = regexNumber.Matches(context[index].ToString());
            if (matchesTwo.Count > 0)
            {
                return NumberCheck(context, index, max);
            }
            else
            {
                Console.WriteLine("Произошла ошибка: index = " + (index + 1) + " symbol = " + context[index]);
                return false;
            }
        }
    }
}
