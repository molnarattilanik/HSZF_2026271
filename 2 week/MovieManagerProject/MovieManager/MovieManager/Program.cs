namespace MovieManager
{
    internal class Program
    {
        public sealed class Movie
        {
            public string Title { get; set; } = string.Empty;
            public int Length { get; set; }
            public int Published { get; set; }
        }

        static void Main(string[] args)
        {
            List<Movie> movies = new List<Movie>();
            bool isRunning = true;

            do
            {
                //egyszerűség kedvéért a felhasználótól egyetlen sorban várjuk a bemenetet, amely tartalmazza a filmet, hosszát és a megjelenés évét pontosvesszővel elválasztva
                Console.WriteLine("Új film felvétele: 'title;length;published'");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("A bemenet üres. Kérem, adjon meg egy érvényes bemenetet.");
                    continue;
                }

                string[] parts = input.Split(';');

                //ez is csak egyszerűsítés, a valóságban érdemes lenne ellenőrizni, hogy a parts tömb hossza 3-e, és hogy a hossz és a megjelenés év szám-e
                Movie movie = new()
                {
                    Title = parts[0],
                    Length = int.Parse(parts[1]),
                    Published = int.Parse(parts[2])
                };

                movies.Add(movie);

                Console.WriteLine("Szeretnél újabb filmet felvenni? (igen/nem)");
                string response = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(response) && response.Trim().Equals("nem", StringComparison.CurrentCultureIgnoreCase))
                {
                    isRunning = false;
                }
            } while (isRunning);
        }
    }
}
