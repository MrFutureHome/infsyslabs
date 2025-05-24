using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    public sealed class LoggerConfiguration
    {
        internal LogLevel MinimumLevel { get; private set; } = LogLevel.Info;
        internal string Template { get; private set; } = "{t} | {L} | {f}:{n} -> {m}";
        internal List<ILogSink> Sinks { get; } = new();

        // минимальный уровень логирования
        public LoggerConfiguration SetMinimumLevel(LogLevel level)
        {
            MinimumLevel = level;
            return this;
        }

        // конфигуратор шаблона логирования
        public LoggerConfiguration SetTemplate(string template)
        {
            Template = template;
            return this;
        }

        // консольный вывод
        public LoggerConfiguration AddConsole()
        {
            Sinks.Add(new ConsoleSink());
            return this;
        }

        // файловый вывод
        // fileName - имя файла (если null — будет сгенерировано уникальное)
        // append - True = дозаписывать, False = перезаписывать
        public LoggerConfiguration AddFile(string? fileName = null, bool append = true)
        {
            Sinks.Add(new FileSink(fileName, append));
            return this;
        }
    }
}
