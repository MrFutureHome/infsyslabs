using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InitializeMenu();

            //Settings defaultsettings = new Settings(true, true, true);
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

            if (int.TryParse(Console.ReadLine(), out int value)) 
            {
                chosenOption = value;
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
            else
            {
                ResetToMenu();
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

            Console.WriteLine("------------- Информация о разделах --------");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                Console.WriteLine("Емкость: {0} ГБ", Math.Round(System.Convert.ToDouble(queryObj["Capacity"]) / 1024 / 1024 / 1024, 2));
                Console.WriteLine("Буква: {0}", queryObj["DriveLetter"]);
                Console.WriteLine("№ диска: {0}", queryObj["DriveType"]);
                Console.WriteLine("Файловая система: {0}", queryObj["FileSystem"]);
                Console.WriteLine("Свободное пространство: {0} ГБ", Math.Round(System.Convert.ToDouble(queryObj["FreeSpace"]) / 1024 / 1024 / 1024, 2));
                Console.WriteLine("-------------------------------------");
            }

            Console.WriteLine("\nНажмите ESC для возврата в меню");

            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();
            }
            while (keyinfo.Key != ConsoleKey.Escape);

            ResetToMenu();
        }

        private static void SystemMonitoring()
        {
            Console.Clear();

            //Console.WriteLine("{0,-40} {1,-20} {2,-15} {3,-10}", "Процесс", "CPU(%)", "ОЗУ (МБ)", "Диск (МБ/с)");
            //Console.WriteLine(new string('-', 100));

            string header = "{0,-40}";
            string separator = new string('-', 40);
            var headerValues = new List<object> { "Процесс" };

            int index = 1; // Начинаем с индекса 1 для CPU

            if (Settings.defaultSettings.CPUUsage)
            {
                header += $" {{{index++},-20}}";
                headerValues.Add("CPU(%)");
                separator += new string('-', 20);
            }

            if (Settings.defaultSettings.MemoryUsage)
            {
                header += $" {{{index++},-15}}";
                headerValues.Add("ОЗУ (МБ)");
                separator += new string('-', 15);
            }

            if (Settings.defaultSettings.DiscUsage)
            {
                header += $" {{{index++},-10}}";
                headerValues.Add("Диск (МБ/с)");
                separator += new string('-', 10);
            }

            // Выводим заголовок и разделитель
            Console.WriteLine(header, headerValues.ToArray());
            Console.WriteLine(separator);

            // Получаем список всех процессов
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                try
                {
                    // Начинаем со списка значений, содержащего только название процесса
                    var values = new List<object> { process.ProcessName };

                    if (Settings.defaultSettings.CPUUsage)
                    {
                        // Использование CPU
                        PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName, true);
                        cpuCounter.NextValue(); // Первая выборка
                        Thread.Sleep(100); // Задержка для получения реальных данных
                        values.Add(Math.Round(cpuCounter.NextValue() / Environment.ProcessorCount, 2));
                    }

                    if (Settings.defaultSettings.MemoryUsage)
                    {
                        // Использование памяти
                        values.Add(Math.Round(process.PrivateMemorySize64 / 1024.0 / 1024.0, 2)); // МБ
                    }

                    if (Settings.defaultSettings.DiscUsage)
                    {
                        // Использование диска
                        PerformanceCounter diskCounter = new PerformanceCounter("Process", "IO Data Bytes/sec", process.ProcessName, true);
                        values.Add(Math.Round(diskCounter.NextValue() / 1024 / 1024, 2)); // МБ/с
                    }

                    Console.WriteLine(header, values.ToArray());
                }
                catch
                {
                    // Игнорируем процессы, к которым нет доступа или которые были завершены
                    continue;
                }
            }

            Console.WriteLine(new string('-', 100));
            Console.SetCursorPosition(0, Console.CursorTop);

            Console.WriteLine("\nНажмите ESC для возврата в меню");

            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();
            }
            while (keyinfo.Key != ConsoleKey.Escape);

            ResetToMenu();
        }

        private static void ProgramSettings()
        {
            Console.Clear();

            int chosenSetting = 0;

            Settings.defaultSettings.returnCurrentSettings();

            Console.WriteLine("Выберите опцию для редактирования или нажмите любую клавишу для возврата в меню");

            ConsoleKeyInfo keyinfo;
            keyinfo = Console.ReadKey();
            
            if (int.TryParse(keyinfo.KeyChar.ToString(), out int value))
            {
                chosenSetting = value;
                switch (chosenSetting)
                {
                    case 1:
                        Settings.defaultSettings.CPUUsage = !Settings.defaultSettings.CPUUsage;
                        ProgramSettings();
                        break;
                    case 2:
                        Settings.defaultSettings.MemoryUsage = !Settings.defaultSettings.MemoryUsage;
                        ProgramSettings();
                        break;
                    case 3:
                        Settings.defaultSettings.DiscUsage = !Settings.defaultSettings.DiscUsage;
                        ProgramSettings();
                        break;
                    default:
                        ProgramSettings();
                        break;
                }
            }
            
            else
            {
                ResetToMenu();
            }
        }

        private static void SmartMonitoring()
        {
            Console.Clear();

            ResetToMenu();
        }

    }
}
