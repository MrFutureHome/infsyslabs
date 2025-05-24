using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    internal sealed class ConsoleSink : ILogSink
    {
        private static readonly object _consoleLock = new();

        public void Write(string formattedMessage)
        {
            lock (_consoleLock)
            {
                Console.OutputEncoding = Encoding.UTF8;
                Console.WriteLine(formattedMessage);
            }
        }

        public void Dispose() { }
    }
}
