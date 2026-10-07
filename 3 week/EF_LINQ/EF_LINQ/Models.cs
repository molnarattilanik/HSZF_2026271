namespace EF_LINQ;

public class Director
{
    public Director() { }
    public Director(int id, string name) => (Id, Name) = (id, name);
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}

public class Movie
{
    public Movie() { }
    public Movie(int id, string title, double income, int directorId, DateTime published, double rating) =>
        (Id, Title, Income, DirectorId, Published, Rating) = (id, title, income, directorId, published, rating);
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public double Income { get; set; }
    public DateTime Published { get; set; }
    public double Rating { get; set; }

    public int DirectorId { get; set; }
    public virtual Director Director { get; set; } = null!;
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}

public class Actor
{
    public Actor() { }
    public Actor(int id, string name) => (Id, Name) = (id, name);
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}

public class Role
{
    public Role() { }
    public Role(int id, int movieId, int actorId, int rank, string character) =>
        (Id, MovieId, ActorId, Rank, Character) = (id, movieId, actorId, rank, character);
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int ActorId { get; set; }
    public int Rank { get; set; }
    public string Character { get; set; } = string.Empty;

    public virtual Movie Movie { get; set; } = null!;
    public virtual Actor Actor { get; set; } = null!;
}
