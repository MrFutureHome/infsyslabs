using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    public sealed class Logger : IDisposable
    {
        private static readonly Lazy<Logger> _lazy = new(() => new Logger());
        public static Logger Instance => _lazy.Value;

        private readonly BlockingCollection<(LogLevel Level, string Message, string SourceFile, int SourceLine)> _queue = new();
        private readonly CancellationTokenSource _cts = new();
        private LoggerConfiguration _config = new();

        private Logger()
        {
            Task.Factory.StartNew(ProcessQueue, TaskCreationOptions.LongRunning);
        }

        public void Configure(Action<LoggerConfiguration> configure)
        {
            var cfg = new LoggerConfiguration();
            configure(cfg);
            if (cfg.Sinks.Count == 0) cfg.AddConsole();
            _config = cfg;
        }

        public void Log(LogLevel level, string message, string sourceFile, int sourceLine)
        {
            if (level < _config.MinimumLevel) return;
            _queue.Add((level, message, sourceFile, sourceLine));
        }

        private void ProcessQueue()
        {
            try
            {
                foreach (var item in _queue.GetConsumingEnumerable(_cts.Token))
                {
                    string formatted = TemplateFormatter.Format(_config.Template, item.Level, item.Message, item.SourceFile, item.SourceLine);
                    foreach (var sink in _config.Sinks)
                    {
                        sink.Write(formatted);
                    }
                }
            }
            catch (OperationCanceledException) { }
        }

        public void Dispose()
        {
            _queue.CompleteAdding();
            _cts.Cancel();
            foreach (var sink in _config.Sinks) sink.Dispose();
            _cts.Dispose();
        }
    }
}
