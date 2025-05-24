using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    internal sealed class FileSink : ILogSink
    {
        private readonly StreamWriter _writer;
        private readonly object _fileLock = new();

        public FileSink(string? fileName, bool append)
        {
            var resolvedName = fileName ?? GenerateUniqueFileName("log");
            _writer = new StreamWriter(resolvedName, append, Encoding.UTF8) { AutoFlush = true };
        }

        public void Write(string formattedMessage)
        {
            lock (_fileLock)
            {
                _writer.WriteLine(formattedMessage);
            }
        }

        public void Dispose() => _writer.Dispose();

        private static string GenerateUniqueFileName(string baseName)
        {
            var ts = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            return $"{baseName}_{ts}.log";
        }
    }
}
