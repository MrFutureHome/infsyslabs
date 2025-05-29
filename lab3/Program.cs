namespace lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger.Instance.Configure(cfg => cfg
            .SetMinimumLevel(LogLevel.Trace)
            .SetTemplate("{t} | {L} | {f}:{n} -> {m}")
            .AddConsole()
            .AddFile());

            Log.LOGI("Приложение запущено");

            try
            {
                DoWork();
            }
            catch (Exception ex)
            {
                Log.LOGE("Критическая ошибка: {0}", ex);
            }

            Log.LOGW("Завершаем работу…");
            (Logger.Instance as IDisposable)?.Dispose();
        }

        //тестовый цикл для проверки
        private static void DoWork()
        {
            Log.LOGD("Начало");
            for (int i = 0; i < 3; i++)
            {
                Log.LOGT("Итерация {0}", i);
                Task.Delay(100).Wait();
            }
            Log.LOGI("Программа завершена");
        }
    }
}
