using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab5
{
    public class Movie
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Rating { get; set; }

        public string Genre { get; set; }
        public string PosterUrl { get; set; }
        public string TrailerUrl { get; set; }
        public string Description { get; set; }

        public override string ToString() => $"{Title} ({Year}) — {Rating}/10";
    }
}
