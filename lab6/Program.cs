using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Windows.Forms;

namespace lab6
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            int part = 0;
            int ex = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите часть (1 или 2): ");

                part = Convert.ToInt32(Console.ReadLine());
                switch (part)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Выберите задание");
                        Console.WriteLine("1. Проверка корректности пароля");
                        Console.WriteLine("2. Проверка корректности WEB-цвета");
                        Console.WriteLine("3. Токенизация математического выражения");
                        Console.WriteLine("4. Проверка корректности даты");

                        ex = Convert.ToInt32(Console.ReadLine());
                        switch (ex)
                        {
                            case 1:
                                Console.Clear();
                                part1_1();
                                break;
                            case 2:
                                Console.Clear();
                                part1_2();
                                break;
                            case 3:
                                Console.Clear();
                                part1_3();
                                break;
                            case 4:
                                Console.Clear();
                                part1_4();
                                break;
                        }
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("Выберите задание: ");
                        Console.WriteLine("1. Проверка корректности скобочного выражения");
                        Console.WriteLine("2. Разбиение текста на предложения");

                        ex = Convert.ToInt32(Console.ReadLine());

                        switch (ex)
                        {
                            case 1:
                                Console.Clear();
                                part2_1();
                                break;
                            case 2:
                                Console.Clear();
                                part2_2();
                                break;
                        }
                        break;

                    default:
                        return;
                }

                if (Choose_ex() == true)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

        }

        public static bool Choose_ex()
        {
            Console.WriteLine("\nЗакрыть программу? (Y/N)");
            switch (Console.ReadLine())
            {
                case "N":
                    return true;
                case "Y":
                    return false;
                default:
                    return false;
            }
        }

        public static void part1_1() 
        { 
            Console.WriteLine("Введите пароль");
            string password = Console.ReadLine();
            bool isValid = CheckPassword(password);

            if (isValid == false)
            {
                Console.WriteLine($"Пароль {password} не соответствует требованиям!");
            }
            else
            {
                Console.WriteLine($"Пароль {password} соответствует требованиям!");
            }
        }

        public static void part1_2() 
        {
            Console.WriteLine("Введите цвет");
            string color = Console.ReadLine().TrimEnd();
            bool isValid = CheckColor(color);
        
            if (isValid == false)
            {
                Console.WriteLine($"Цвет {color} задан неправильно!");
            }
            else
            {
                Console.WriteLine($"Цвет {color} задан правильно!");
            }
        }

        public static void part1_3() 
        {
            Console.WriteLine("Введите выражение");
            string expression = Console.ReadLine();
            var tokens = TokenizeExpression(expression);
            foreach (var token in tokens)
            {
                Console.WriteLine($"Type: {token.Type}, Value: {token.Value}");
            }
        }

        public static void part1_4() 
        {
            Console.WriteLine("Введите дату");
            string date = Console.ReadLine();
            bool isValid = CheckDate(date);

            if (isValid == false)
            {
                Console.WriteLine($"Дата {date} не соответствует правильному формату!");
            }
            else
            {
                Console.WriteLine($"Дата {date} соответствует правильному формату!");
            }
        }

        public static void part2_1()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            Console.WriteLine("Выберите файл со скобочными выражениями:");
            openFileDialog.Filter = "Текстовые файлы|*.txt|Все файлы|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] testCases = File.ReadAllLines(openFileDialog.FileName);
                foreach (var testCase in testCases)
                {
                    Console.WriteLine($"{testCase}: {IsValidBracketSequence(testCase)}");
                }
            }
            else
            {
                Console.WriteLine("Файл не был выбран.");
            }
        }

        public static void part2_2() 
        {
            Console.WriteLine("Выберите файл с текстом:");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string text = File.ReadAllText(openFileDialog.FileName);
                FindPersons(text);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Файл не был выбран.");
            }
        }

        public static bool CheckPassword(string password)
        {
            var specialChars = new[] { '^', '$', '%', '@', '#', '&', '*', '!', '?' };
            if (password.Length < 8 || !password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit)
                || password.Count(c => specialChars.Contains(c)) < 2)
            {
                return false;
            }

            for (int i = 1; i < password.Length; i++)
            {
                if (password[i] == password[i - 1])
                    return false;
            }

            return true;
        }
        public static bool CheckColor(string color)
        {
            if (color.StartsWith("#"))
            {
                if (color.Length == 7 || color.Length == 4)
                    return true;
            }
            else if (color.StartsWith("rgb(") && color.EndsWith(")"))
            {
                string content = color.Substring(4, color.Length - 5);
                string[] parts = content.Split(',');
                if (parts.Length == 3)
                {
                    foreach (var part in parts)
                    {
                        string trimmed = part.Trim();
                        if (trimmed.EndsWith("%"))
                        {
                            if (!int.TryParse(trimmed.TrimEnd('%'), out int percent) || percent < 0 || percent > 100)
                                return false;
                        }
                        else
                        {
                            if (!int.TryParse(trimmed, out int value) || value < 0 || value > 255)
                                return false;
                        }
                    }
                    return true;
                }
            }
            else if (color.StartsWith("hsl(") && color.EndsWith(")"))
            {
                string content = color.Substring(4, color.Length - 5);
                string[] parts = content.Split(',');
                if (parts.Length == 3)
                {
                    if (!int.TryParse(parts[0].Trim(), out int h) || h < 0 || h > 360)
                        return false;

                    for (int i = 1; i < 3; i++)
                    {
                        string trimmed = parts[i].Trim();
                        if (!trimmed.EndsWith("%"))
                            return false;

                        if (!int.TryParse(trimmed.TrimEnd('%'), out int percent) || percent < 0 || percent > 100)
                            return false;
                    }
                    return true;
                }
            }
            return false;
        }
        public static bool CheckDate(string date)
        {
            var regex = new Regex(@"^(?:(?:\d{1,2}[./-]\d{1,2}[./-]\d{4})|(?:\d{4}[./-]\d{1,2}[./-]\d{1,2})|
                (?:\d{1,2}\s(?:января|февраля|марта|апреля|мая|июня|июля|августа|сентября|октября|ноября|декабря)\s\d{4})|
                (?:(?:January|February|March|April|May|June|July|August|September|October|November|December)\s\d{1,2},\s\d{4})|
                (?:(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s\d{1,2},\s\d{4})|
                (?:\d{4},\s(?:January|February|March|April|May|June|July|August|September|October|November|December)\s\d{1,2})|
                (?:\d{4},\s(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s\d{1,2}))$");

            return regex.IsMatch(date);
        }
        static List<Token> TokenizeExpression(string expression)
        {
            var tokens = new List<Token>();
            var regex = new Regex(@"(?<function>sin|cos|tg|ctg|tan|cot|sinh|cosh|th|cth|tanh|coth|ln|lg|log|exp|sqrt|cbrt|abs|sign)|(?<constant>pi|e|sqrt2|ln2|ln10)|(?<number>\d+(\.\d+)?)|(?<variable>[a-zA-Z_][a-zA-Z0-9_]*)|(?<operator>[\^*/+-])|(?<left_parenthesis>\()|(?<right_parenthesis>\))");

            var matches = regex.Matches(expression);
            foreach (Match match in matches)
            {
                if (match.Groups["function"].Success)
                    tokens.Add(new Token("function", match.Value));
                else if (match.Groups["constant"].Success)
                    tokens.Add(new Token("constant", match.Value));
                else if (match.Groups["number"].Success)
                    tokens.Add(new Token("number", match.Value));
                else if (match.Groups["variable"].Success)
                    tokens.Add(new Token("variable", match.Value));
                else if (match.Groups["operator"].Success)
                    tokens.Add(new Token("operator", match.Value));
                else if (match.Groups["left_parenthesis"].Success)
                    tokens.Add(new Token("left_parenthesis", match.Value));
                else if (match.Groups["right_parenthesis"].Success)
                    tokens.Add(new Token("right_parenthesis", match.Value));
            }

            return tokens;
        }
        static bool IsValidBracketSequence(string input)
        {
            var regex = new Regex("\\(\\)|\\{\\}|\\[\\]");
            while (regex.IsMatch(input))
            {
                input = regex.Replace(input, "");
            }
            return string.IsNullOrEmpty(input);
        }
        static void FindPersons(string text)
        {           
            string[] words = text.Split(new[] { ' ', '.', ',', ';', ':', '"', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            ConsoleColor currentColor = ConsoleColor.White;

            for (int i = 0; i < words.Length; i++)
            {
                if (i + 1 < words.Length)
                {
                    bool isFirstWordPerson = IsPersonName(words[i]);
                    bool isSecondWordPerson = IsPersonName(words[i + 1]);

                    if (isFirstWordPerson && isSecondWordPerson)
                    {
                        currentColor = currentColor == ConsoleColor.White ? ConsoleColor.Cyan : ConsoleColor.White;

                        Console.ForegroundColor = currentColor;
                        Console.Write(words[i] + " " + words[i + 1] + " ");

                        i++;
                        continue;
                    }
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(words[i] + " ");
            }

            Console.ResetColor();
        }

        static bool IsPersonName(string word)
        {
            if (string.IsNullOrEmpty(word)) return false;
            if (char.IsUpper(word[0]))
            {
                for (int i = 1; i < word.Length; i++)
                {
                    if (!char.IsLower(word[i]) && word[i] != 'Ё' && word[i] != 'ё')
                        return false;
                }
                return true;
            }
            return false;
        }

    }
    class Token
    {
        public string Type { get; }
        public string Value { get; }

        public Token(string type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
