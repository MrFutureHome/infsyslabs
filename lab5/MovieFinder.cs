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
        private static readonly string tmdbApiKey = ApiKeys.TmdbApiKey;
        private static readonly string omdbApiKey = ApiKeys.OmdbApiKey;

        public static async Task<List<Movie>> FindMoviesAsync(int genreId, double minRating, int yearFrom, int yearTo)
        {
            var movies = new List<Movie>();
            var seenImdbIds = new HashSet<string>();

            using (HttpClient client = new HttpClient())
            {
                for (int page = 1; page <= 3; page++)
                {
                    string url = $"https://api.themoviedb.org/3/discover/movie" +
                                 $"?api_key={tmdbApiKey}" +
                                 $"&with_genres={genreId}" +
                                 $"&vote_average.gte={minRating.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                                 $"&primary_release_date.gte={yearFrom}-01-01" +
                                 $"&primary_release_date.lte={yearTo}-12-31" +
                                 $"&sort_by=popularity.desc" +
                                 $"&language=ru-RU" +
                                 $"&page={page}";

                    var response = await client.GetStringAsync(url);
                    JObject data = JObject.Parse(response);

                    foreach (var tmdbMovie in data["results"])
                    {
                        int tmdbId = tmdbMovie["id"].Value<int>();

                        string externalUrl = $"https://api.themoviedb.org/3/movie/{tmdbId}/external_ids?api_key={tmdbApiKey}";
                        var extResponse = await client.GetStringAsync(externalUrl);
                        JObject externalData = JObject.Parse(extResponse);
                        string imdbId = externalData["imdb_id"]?.ToString();

                        if (string.IsNullOrEmpty(imdbId) || seenImdbIds.Contains(imdbId))
                            continue;

                        seenImdbIds.Add(imdbId);

                        var movie = await GetOmdbMovieByIdAsync(imdbId);
                        if (movie != null)
                            movies.Add(movie);
                    }
                }
            }

            return movies;
        }

        public static async Task<Movie> GetOmdbMovieByIdAsync(string imdbId)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"http://www.omdbapi.com/?apikey={omdbApiKey}&i={imdbId}&plot=full&r=json";
                var response = await client.GetStringAsync(url);
                JObject data = JObject.Parse(response);

                if (data["Response"]?.ToString() == "True")
                {
                    return new Movie
                    {
                        Title = data["Title"]?.ToString(),
                        Year = data["Year"]?.ToString(),
                        Genre = data["Genre"]?.ToString(),
                        Description = data["Plot"]?.ToString(),
                        Rating = data["imdbRating"]?.ToString(),
                        PosterUrl = data["Poster"]?.ToString(),
                        TrailerUrl = $"https://www.imdb.com/title/{imdbId}/"
                    };
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
