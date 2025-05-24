using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab5
{
    public static class ApiKeys
    {
        public static string OmdbApiKey { get; private set; }
        public static string TmdbApiKey { get; private set; }

        public static void LoadKeys(string filePath = "apikeys.txt")
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл с ключами не найден: " + filePath);

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('=');
                if (parts.Length != 2) continue;

                var key = parts[0].Trim();
                var value = parts[1].Trim();

                if (key == "OMDB")
                    OmdbApiKey = value;
                else if (key == "TMDB")
                    TmdbApiKey = value;
            }

            if (string.IsNullOrEmpty(OmdbApiKey) || string.IsNullOrEmpty(TmdbApiKey))
                throw new InvalidOperationException("Какой-то из ключей не был найден. Пу-пу-пу");
        }
    }
}
