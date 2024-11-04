using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    internal class Settings
    {
        private bool _cpuusage;
        private bool _memoryusage;
        private bool _discusage;

        public bool CPUUsage 
        {
            get
            {
                return _cpuusage;
            }
            set
            {
                this._cpuusage = value;
            } 
        }
        public bool MemoryUsage
        {
            get
            {
                return _memoryusage;
            }
            set
            {
                this._memoryusage = value;
            }
        }
        public bool DiscUsage
        {
            get
            {
                return _discusage;
            }
            set
            {
                this._discusage = value;
            }
        }

        public Settings(bool CPUUsage, bool MemoryUsage, bool DiscUsage)
        {
            this.CPUUsage = CPUUsage;
            this.MemoryUsage = MemoryUsage;
            this.DiscUsage = DiscUsage;
        }

        public static Settings defaultSettings = new Settings(true, true, true);

        public void returnCurrentSettings()
        {
            Console.WriteLine($"1. Использование CPU: {_cpuusage}\n" +
                $"2. Использование ОЗУ: {_memoryusage}\n" +
                $"3. Использование диска: {_discusage}\n");
        }

    }
}
