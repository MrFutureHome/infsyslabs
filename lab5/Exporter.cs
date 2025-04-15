using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace lab5
{
    public static class Exporter
    {
        public static void ExportToJson(List<Movie> movies, string path)
        {
            string json = JsonConvert.SerializeObject(movies, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public static void ExportToCsv(List<Movie> movies, string path)
        {
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(movies);
            }
        }

        public static string GenerateShareLink(List<Movie> movies)
        {
            if (movies == null || movies.Count == 0)
                return string.Empty;

            var titles = movies.Take(5).Select(m => m.Title).ToList();
            string query = string.Join(", ", titles);
            string encoded = HttpUtility.UrlEncode(query);

            return $"https://www.google.com/search?q=movies+like+{encoded}";
        }
    }
}
