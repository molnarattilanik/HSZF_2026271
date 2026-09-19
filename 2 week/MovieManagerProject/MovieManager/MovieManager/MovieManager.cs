using System.Text.Json;

namespace MovieManager
{
    public interface IMovieManager
    {
        event EventHandler<string>? OutputGenerated;
        void AddMovie(Movie movie);
        List<Movie> GetAllMovies();
        void SaveToJson();
        void MoviesAfter2000();
        void MoviesLongerThanTwoHours();
        void AnyMovieMoreThanOneHOur();
        void AllMoviesLongerThanHalfAnHour();
        void FirstMovieAfter2000AndMoreThanTwoHour();
    }

    public sealed class MovieManager : IMovieManager
    {
        public event EventHandler<string>? OutputGenerated;

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

        public void MoviesAfter2000()
        {
            var moviesAfter2000 = movies.Where(m => m.Published > 2000).ToList();
            foreach (var movie in moviesAfter2000)
            {
                OnOutputGenerated($"Title: {movie.Title}, Length: {movie.Length}, Published: {movie.Published}");
            }
        }

        public void MoviesLongerThanTwoHours()
        {
            var longMovies = movies.Where(m => m.Length > 120).ToList();
            foreach (var movie in longMovies)
            {
                OnOutputGenerated($"Title: {movie.Title}, Length: {movie.Length}, Published: {movie.Published}");
            }
        }

        public void AnyMovieMoreThanOneHOur()
        {
            bool anyMovieMoreThanOneHour = movies.Any(m => m.Length > 60);
            OnOutputGenerated($"Is there any movie longer than one hour? {anyMovieMoreThanOneHour}");
        }

        public void AllMoviesLongerThanHalfAnHour()
        {
            bool allMoviesLongerThanHalfAnHour = movies.All(m => m.Length > 30);
            OnOutputGenerated($"Are all movies longer than half an hour? {allMoviesLongerThanHalfAnHour}");
        }

        public void FirstMovieAfter2000AndMoreThanTwoHour()
        {
            var firstMovie = movies.FirstOrDefault(m => m.Published > 2000 && m.Length > 120);
            if (firstMovie != null)
            {
                OnOutputGenerated($"First movie after 2000 and longer than one hour: Title: {firstMovie.Title}, Length: {firstMovie.Length}, Published: {firstMovie.Published}");
            }
            else
            {
                OnOutputGenerated("No movie found after 2000 and longer than one hour.");
            }
        }

        private void OnOutputGenerated(string message)
        {
            OutputGenerated?.Invoke(this, message);
        }
    }
}
