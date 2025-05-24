using lab3; // проект с логгером

namespace lab3__demo_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger.Instance.Configure(cfg => cfg
                .SetMinimumLevel(LogLevel.Debug)          // показываем всё с DEBUG и выше
                .SetTemplate("{t} [{L}] {f}:{n}  {m}")    // свой шаблон
                .AddConsole()                             // в консоль
                .AddFile());                              // и в файл log_yyyy-MM-dd_HH-mm-ss.log

            Log.LOGI("Demo запущена ✅");

            // имитация многопоточной нагрузки
            var tasks = Enumerable.Range(0, 5)
                                  .Select(i => Task.Run(() => Worker(i)))
                                  .ToArray();

            Task.WaitAll(tasks);

            Log.LOGW("Demo завершена");

            // закрытие логгера
            (Logger.Instance as IDisposable)?.Dispose();
        }

        private static void Worker(int id)
        {
            Log.LOGD("Поток {0} начал работу", id);

            for (int j = 0; j < 3; j++)
            {
                Log.LOGT("Поток {0} – итерация {1}", id, j);
                Task.Delay(Random.Shared.Next(100, 400)).Wait();
            }

            if (id == 2)                           // специальная ошибка для проверки
                Log.LOGE("Поток {0} столкнулся с ошибкой", id);

            Log.LOGI("Поток {0} завершён", id);
        }
    }
}
