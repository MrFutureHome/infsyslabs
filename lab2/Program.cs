using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int chosenOption = 0;
            InitializeMenu();

            
        }

        private static void InitializeMenu()
        {
            Console.Clear();
            int chosenOption = 0;

            Console.WriteLine("Выберите опцию: \n" +
                "1. Характеристики системы\n" +
                "2. Мониторинг процессов\n" +
                "3. Настройки\n" +
                "4. SMART\n" +
                ">> ");

            chosenOption = int.Parse(Console.ReadLine());

            switch (chosenOption)
            {
                case 1:
                    SystemSpecs();
                    break;
                case 2:
                    SystemMonitoring();
                    break;
                case 3:
                    ProgramSettings();
                    break;
                case 4:
                    SmartMonitoring();
                    break;
                default:
                    ResetToMenu();
                    break;
            }
        }

        private static void ResetToMenu()
        {
            Console.Clear();
            InitializeMenu();
        }

        private static void SystemSpecs()
        {
            Console.Clear();

            ManagementObjectSearcher searcher8 =
                new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_Processor");

            Console.WriteLine("------------- Список обнаруженных CPU ---------------");

            foreach (ManagementObject queryObj in searcher8.Get())
            {
                Console.WriteLine("CPU: {0}", queryObj["Name"]);
                Console.WriteLine("Число ядер: {0}", queryObj["NumberOfCores"]);
                Console.WriteLine("Частота: {0} МГц", queryObj["MaxClockSpeed"]);
            }

            Console.WriteLine("\n");

            ManagementObjectSearcher searcher11 =
                new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_VideoController");

            Console.WriteLine("----------- Список обнаруженных GPU -----------");

            foreach (ManagementObject queryObj in searcher11.Get())
            {
                
                long ramInBytes = Convert.ToInt64(queryObj["AdapterRAM"]);
                Console.WriteLine("Название: {0}", queryObj["Caption"]);
                Console.WriteLine("Кол-во памяти: {0} МБ", (ramInBytes / 1048576));
                Console.WriteLine("Семейство: {0}", queryObj["VideoProcessor"]);
                Console.WriteLine("-------------------------------------");
            }

            Console.WriteLine("\n");

            ManagementObjectSearcher searcher12 =
                new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_PhysicalMemory");

            Console.WriteLine("------------- Информация об оперативной памяти --------");
            foreach (ManagementObject queryObj in searcher12.Get())
            {
                Console.WriteLine("Модуль: {0} ; Объём: {1} ГБ; Скорость: {2} МГц", queryObj["BankLabel"],
                                  Math.Round(System.Convert.ToDouble(queryObj["Capacity"]) / 1024 / 1024 / 1024, 2),
                                   queryObj["Speed"]);
            }

            Console.WriteLine("\n");

            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_Volume");

            Console.WriteLine("------------- Информация о дисках --------");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                Console.WriteLine("Емкость: {0} ГБ", Math.Round(System.Convert.ToDouble(queryObj["Capacity"]) / 1024 / 1024 / 1024, 2));
                Console.WriteLine("DriveLetter: {0}", queryObj["DriveLetter"]);
                //Console.WriteLine("DriveType: {0}", queryObj["DriveType"]);
                Console.WriteLine("Файловая система: {0}", queryObj["FileSystem"]);
                Console.WriteLine("Свободное пространство: {0} ГБ", Math.Round(System.Convert.ToDouble(queryObj["FreeSpace"]) / 1024 / 1024 / 1024, 2));
                Console.WriteLine("-------------------------------------");
            }

            Console.WriteLine("\nНажмите ESC для возврата в меню");

            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                //Console.WriteLine(keyinfo.Key + " was pressed");
            }
            while (keyinfo.Key != ConsoleKey.Escape);

            ResetToMenu();
        }

        private static void SystemMonitoring()
        {
            Console.Clear();

            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2",
           "Select Name, CommandLine From Win32_Process");

            foreach (ManagementObject instance in searcher.Get())
            {
                Console.WriteLine("{0}", instance["Name"]);
            }

            Console.WriteLine("\nНажмите ESC для возврата в меню");

            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                //Console.WriteLine(keyinfo.Key + " was pressed");
            }
            while (keyinfo.Key != ConsoleKey.Escape);

            ResetToMenu();
        }

        private static void ProgramSettings()
        {
            Console.Clear();

            ResetToMenu();
        }

        private static void SmartMonitoring()
        {
            Console.Clear();

            ResetToMenu();
        }

    }
}
