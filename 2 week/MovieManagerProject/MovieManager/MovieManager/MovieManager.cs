using System.Text.Json;

namespace MovieManager
{
    public interface IMovieManager
    {
        void AddMovie(Movie movie);
        List<Movie> GetAllMovies();
        void SaveToJson();
    }

    public sealed class MovieManager : IMovieManager
    {
        //Copy if newer-re kell állítani, Copy always felül írja arra a fájlt, ami a projektben van, így a program futtatásakor mindig az üres fájl lesz a kimenet, és nem az előzőleg mentett adatok
        private readonly string filePath = "movies.json";
        private List<Movie> movies = new List<Movie>();

        public MovieManager()
        {
            movies = LoadFromJson();
        }

        public void AddMovie(Movie movie)
        {
            movies.Add(movie);
        }

        public List<Movie> GetAllMovies()
        {
            return movies;
        }

        public void SaveToJson()
        {
            string json = JsonSerializer.Serialize(movies);
            File.WriteAllText(filePath, json);
        }

        private List<Movie> LoadFromJson()
        {
            string json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<Movie>();
            }
            return JsonSerializer.Deserialize<List<Movie>>(json);
        }
    }
}
