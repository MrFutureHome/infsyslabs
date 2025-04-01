using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

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
                            default:
                                return;
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

            bool isValid = IsDateValid(date);
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
                string text = File.ReadAllText(openFileDialog.FileName).Replace("\r", "").Replace("\n", "");
                CheckText(text);
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

        static bool IsDateValid(string date)
        {
            var match = Regex.Match(date,
                @"^(?:(?<day>\d{1,2})[./-](?<month>\d{1,2})[./-](?<year>\d{4})" +
                @"|(?<year>\d{4})[./-](?<month>\d{1,2})[./-](?<day>\d{1,2})" +
                @"|(?<day>\d{1,2})\s(?<monthRus>января|февраля|марта|апреля|мая|июня|июля|августа|сентября|октября|ноября|декабря)\s(?<year>\d{4})" +
                @"|(?<monthEng>January|February|March|April|May|June|July|August|September|October|November|December)\s(?<day>\d{1,2}),\s(?<year>\d{4})" +
                @"|(?<monthEngShort>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s(?<day>\d{1,2}),\s(?<year>\d{4})" +
                @"|(?<year>\d{4}),\s(?<monthEng>January|February|March|April|May|June|July|August|September|October|November|December)\s(?<day>\d{1,2})" +
                @"|(?<year>\d{4}),\s(?<monthEngShort>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s(?<day>\d{1,2}))$");

            if (!match.Success) return false;

            int day = int.Parse(match.Groups["day"].Value);
            int year = int.Parse(match.Groups["year"].Value);
            int month;

            if (match.Groups["month"].Success)
            {
                month = int.Parse(match.Groups["month"].Value);
            }
            else if (match.Groups["monthRus"].Success)
            {
                month = GetMonthFromRussian(match.Groups["monthRus"].Value);
            }
            else if (match.Groups["monthEng"].Success)
            {
                month = GetMonthFromEnglish(match.Groups["monthEng"].Value);
            }
            else
            {
                month = GetMonthFromEnglishAbbreviation(match.Groups["monthEngShort"].Value);
            }

            return year >= 0 && month >= 1 && month <= 12 && day >= 1 && day <= DateTime.DaysInMonth(year, month);
        }

        static int GetMonthFromRussian(string month)
        {
            switch (month.ToLower())
            {
                case "января": return 1;
                case "февраля": return 2;
                case "марта": return 3;
                case "апреля": return 4;
                case "мая": return 5;
                case "июня": return 6;
                case "июля": return 7;
                case "августа": return 8;
                case "сентября": return 9;
                case "октября": return 10;
                case "ноября": return 11;
                case "декабря": return 12;
                default: return -1;
            }
        }

        static int GetMonthFromEnglish(string month)
        {
            switch (month.ToLower())
            {
                case "january": return 1;
                case "february": return 2;
                case "march": return 3;
                case "april": return 4;
                case "may": return 5;
                case "june": return 6;
                case "july": return 7;
                case "august": return 8;
                case "september": return 9;
                case "october": return 10;
                case "november": return 11;
                case "december": return 12;
                default: return -1;
            }
        }

        static int GetMonthFromEnglishAbbreviation(string month)
        {
            switch (month.ToLower())
            {
                case "jan": return 1;
                case "feb": return 2;
                case "mar": return 3;
                case "apr": return 4;
                case "may": return 5;
                case "jun": return 6;
                case "jul": return 7;
                case "aug": return 8;
                case "sep": return 9;
                case "oct": return 10;
                case "nov": return 11;
                case "dec": return 12;
                default: return -1;
            }
        }

        static void CheckText(string text)
        {
            string pattern = @"(?<sentence>
            (?:
                [^.!?…:]+:                # Захватываем текст до двоеточия (предшествующий список)
                (?:\s*\n\s*)?             # Допустимые пробелы и переводы строк
                (?:
                    \d+\.\s+             # Маркер пункта списка (например, '1. ')
                    (?:(?!\d+\.\s).)+?    # Текст пункта (до появления следующего маркера)
                    (?:;|\.)             # Разделитель: ';' для пункта или '.' для последнего пункта
                    (?:\s*\n\s*)?        # Пробелы и переводы строк между пунктами
                )+
            )
            |
            (?:
                [А-ЯЁA-Z]                # Обычное предложение начинается с буквы
                .+?                      # Минимальное количество символов
                (?:[.!?…]+)             # Завершающий разделитель (один или несколько символов)
                (?=\s|$)                # За разделителем – пробел или конец строки
            )
        )";

            // Создаём регулярное выражение с нужными опциями:
            // IgnorePatternWhitespace позволяет использовать пробелы и комментарии внутри шаблона.
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace);

            // Применяем регулярное выражение и выводим найденные предложения
            MatchCollection matches = regex.Matches(text);
            foreach (Match m in matches)
            {
                Console.WriteLine("Найденное предложение:\n" + m.Groups["sentence"].Value.Trim() + "\n");
            }
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
