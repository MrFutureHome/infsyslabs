using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    internal class Program
    {
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
            Console.WriteLine("Вернуться? (Y/N)");
            switch (Console.ReadLine())
            {
                case "Y":
                    return true;
                case "N":
                    return false;
                default:
                    return false;
            }
        }

        public static void part1_1() 
        { 
            Console.WriteLine("Задание 1 тест");
        }
        public static void part1_2() 
        {
            Console.WriteLine("Задание 2 тест");
        }
        public static void part1_3() 
        {
            Console.WriteLine("Задание 3 тест");
        }
        public static void part1_4() 
        {
            Console.WriteLine("Задание 4 тест");
        }

        public static void part2_1()
        {
            Console.WriteLine("Задание 1 тест");
        }

        public static void part2_2() 
        {
            Console.WriteLine("Задание 2 тест");
        }

    }
}
