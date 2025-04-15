using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab5
{
    public class TmdbGenre
    {
        public int id { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return name;
        }
    }

    public class TmdbGenreResponse
    {
        public List<TmdbGenre> genres { get; set; }
    }
}
