using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    public static class Log
    {
        public static void LOGT(string message, params object[] args) => Write(LogLevel.Trace, message, args);
        public static void LOGD(string message, params object[] args) => Write(LogLevel.Debug, message, args);
        public static void LOGI(string message, params object[] args) => Write(LogLevel.Info, message, args);
        public static void LOGW(string message, params object[] args) => Write(LogLevel.Warning, message, args);
        public static void LOGE(string message, params object[] args) => Write(LogLevel.Error, message, args);

        private static void Write(LogLevel level, string message, object[] args,
            [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            string full = args.Length == 0 ? message : string.Format(message, args);
            Logger.Instance.Log(level, full, file, line);
        }
    }
}
