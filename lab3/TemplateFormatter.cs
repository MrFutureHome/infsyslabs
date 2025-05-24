using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    internal static class TemplateFormatter
    {
        public static string Format(string template, LogLevel level, string message, string file, int line)
        {
            return template
                .Replace("{t}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
                .Replace("{L}", level.ToString().ToUpper())
                .Replace("{m}", message)
                .Replace("{f}", Path.GetFileName(file))
                .Replace("{n}", line.ToString());
        }
    }
}
