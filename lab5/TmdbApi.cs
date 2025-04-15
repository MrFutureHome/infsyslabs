using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace lab5
{
    public static class TmdbApi
    {
        private static readonly string apiKey = "c622c5706cd487c4bb75f6168640c465";
        private static readonly string genreUrl = $"https://api.themoviedb.org/3/genre/movie/list?api_key={apiKey}&language=ru-RU";

        public static async Task<List<TmdbGenre>> GetGenresAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync(genreUrl);
                var genreResponse = JsonConvert.DeserializeObject<TmdbGenreResponse>(response);
                return genreResponse.genres;
            }
        }
    }
}
