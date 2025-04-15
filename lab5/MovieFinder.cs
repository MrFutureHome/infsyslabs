using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace lab5
{
    public static class MovieFinder
    {
        private static readonly string tmdbApiKey = "c622c5706cd487c4bb75f6168640c465";
        private static readonly string omdbApiKey = "6809ff7f";

        public static async Task<List<Movie>> FindMoviesAsync(int genreId, double minRating, int yearFrom, int yearTo)
        {
            var movies = new List<Movie>();

            using (HttpClient client = new HttpClient())
            {
                for (int page = 1; page <= 10; page++)
                {
                    string url = $"https://api.themoviedb.org/3/discover/movie?api_key={tmdbApiKey}&with_genres={genreId}&sort_by=popularity.desc&page={page}&language=ru-RU";
                    var response = await client.GetStringAsync(url);
                    JObject data = JObject.Parse(response);

                    foreach (var tmdbMovie in data["results"])
                    {
                        double? rating = tmdbMovie["vote_average"]?.Value<double>();
                        int? year = ParseYear(tmdbMovie["release_date"]?.ToString());

                        if (rating >= minRating && year >= yearFrom && year <= yearTo)
                        {
                            string title = tmdbMovie["title"].ToString();
                            var movie = await GetOmdbMovieAsync(title);

                            if (movie != null)
                                movies.Add(movie);
                        }
                    }
                }
            }

            return movies;
        }

        private static async Task<Movie> GetOmdbMovieAsync(string title)
        {
            using (HttpClient client = new HttpClient())
            {
                string encodedTitle = HttpUtility.UrlEncode(title);
                string url = $"http://www.omdbapi.com/?t={encodedTitle}&apikey={omdbApiKey}";
                var response = await client.GetStringAsync(url);
                JObject data = JObject.Parse(response);

                if (data["Response"]?.ToString() == "True")
                {
                    return new Movie
                    {
                        Title = data["Title"]?.ToString(),
                        Year = data["Year"]?.ToString(),
                        Rating = data["imdbRating"]?.ToString(),
                        PosterUrl = data["Poster"]?.ToString(),
                        Description = data["Plot"]?.ToString(),
                        TrailerUrl = $"https://www.youtube.com/results?search_query={HttpUtility.UrlEncode(data["Title"]?.ToString() + " trailer")}"
                    };
                }

                return null;
            }
        }

        private static int? ParseYear(string date)
        {
            if (DateTime.TryParse(date, out DateTime parsed))
                return parsed.Year;
            return null;
        }
    }
}
