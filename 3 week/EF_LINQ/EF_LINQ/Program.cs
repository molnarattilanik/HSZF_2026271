using Microsoft.EntityFrameworkCore;

namespace EF_LINQ;

internal static class Program
{
    private static void Main()
    {
        var context = new MovieDbContext();

        var all = context.Actors.ToList();

        QueryMoviesWithDirectors(context);
        QueryAllActorRoles(context);
        QueryMoviesWithCastCount(context);
        QueryActorMovieRelationshipWithJoin(context);
        QueryActorMovieRelationshipWithNavigation(context);
        QueryDirectorStatistics(context);
        QueryMostEmployedActors(context);
        QueryActorPairsInCommon(context);
        QueryDirectorsMostSuccessfulMovie(context);
    }

    private static void QueryMoviesWithDirectors(MovieDbContext db)
    {
        var result = db.Movies.AsNoTracking().OrderBy(m => m.Published).ThenByDescending(m => m.Rating)
            .Select(m => new
            {
                FilmCime = m.Title,
                MegjelenesiEv = m.Published.Year,
                RendezoNeve = m.Director.Name,
                Bevetel = m.Income,
                Ertekeles = m.Rating
            }).ToList();
        Print("1. Filmek rendezői adatokkal", result,
            x => $"{x.FilmCime} | {x.MegjelenesiEv} | {x.RendezoNeve} | {x.Bevetel:0.0} | {x.Ertekeles:0.0}");
    }

    private static void QueryAllActorRoles(MovieDbContext db)
    {
        var result = db.Actors.AsNoTracking().SelectMany(a => a.Roles.Where(r => r.Rank <= 5)
            .Select(r => new
            {
                SzineszNeve = a.Name,
                FilmCime = r.Movie.Title,
                SzerepNeve = r.Character,
                SzerepSorrendje = r.Rank
            })).ToList();
        Print("2. Színészek összes szerepe (top 5)", result,
            x => $"{x.SzineszNeve} | {x.FilmCime} | {x.SzerepNeve} | {x.SzerepSorrendje}");
    }

    private static void QueryMoviesWithCastCount(MovieDbContext db)
    {
        var result = db.Movies.AsNoTracking().Where(m => m.Roles.Count >= 15)
            .OrderByDescending(m => m.Roles.Count)
            .Select(m => new
            {
                FilmCime = m.Title,
                RendezoNeve = m.Director.Name,
                SzereplokSzama = m.Roles.Count,
                Ertekeles = m.Rating
            }).ToList();
        Print("3. Filmek legalább 15 szereplővel", result,
            x => $"{x.FilmCime} | {x.RendezoNeve} | {x.SzereplokSzama} | {x.Ertekeles:0.0}");
    }

    private static void QueryActorMovieRelationshipWithJoin(MovieDbContext db)
    {
        var result = db.Roles.AsNoTracking()
            .Join(db.Actors, r => r.ActorId, a => a.Id, (r, a) => new { r, a })
            .Join(db.Movies, x => x.r.MovieId, m => m.Id, (x, m) => new { x.r, x.a, m })
            .Join(db.Directors, x => x.m.DirectorId, d => d.Id, (x, d) => new
            {
                SzineszNeve = x.a.Name,
                FilmCime = x.m.Title,
                MegjelenesiDatum = x.m.Published,
                RendezoNeve = d.Name,
                SzerepNeve = x.r.Character
            })
            .Where(x => x.MegjelenesiDatum.Year > 2015).ToList();
        Print("4/a. Színész–film kapcsolat Join-nal", result,
            x => $"{x.SzineszNeve} | {x.FilmCime} | {x.MegjelenesiDatum:yyyy-MM-dd} | {x.RendezoNeve} | {x.SzerepNeve}");
    }

    private static void QueryActorMovieRelationshipWithNavigation(MovieDbContext db)
    {
        var result = db.Actors.AsNoTracking().SelectMany(a => a.Roles
            .Where(r => r.Movie.Published.Year > 2015).Select(r => new
            {
                SzineszNeve = a.Name,
                FilmCime = r.Movie.Title,
                MegjelenesiDatum = r.Movie.Published,
                RendezoNeve = r.Movie.Director.Name,
                SzerepNeve = r.Character
            })).ToList();
        Print("4/b. Színész–film kapcsolat navigation propertyvel", result,
            x => $"{x.SzineszNeve} | {x.FilmCime} | {x.MegjelenesiDatum:yyyy-MM-dd} | {x.RendezoNeve} | {x.SzerepNeve}");
    }

    private static void QueryDirectorStatistics(MovieDbContext db)
    {
        var result = db.Movies.AsNoTracking().Include(m => m.Director).Include(m => m.Roles)
            .AsEnumerable().GroupBy(m => new { m.DirectorId, RendezoNeve = m.Director.Name })
            .Where(g => g.Count() >= 2).Select(g => new
            {
                g.Key.RendezoNeve,
                FilmekSzama = g.Count(),
                AtlagosErtekeles = g.Average(m => m.Rating),
                OsszesBevetel = g.Sum(m => m.Income),
                LegnagyobbBevetel = g.Max(m => m.Income)
            }).OrderByDescending(x => x.OsszesBevetel).ToList();
        Print("5. Rendezők statisztikája", result,
            x => $"{x.RendezoNeve} | {x.FilmekSzama} | {x.AtlagosErtekeles:0.00} | {x.OsszesBevetel:0.0} | {x.LegnagyobbBevetel:0.0}");
    }

    private static void QueryMostEmployedActors(MovieDbContext db)
    {
        var result = db.Actors.AsNoTracking().Select(a => new
        {
            SzineszNeve = a.Name,
            FilmekSzama = a.Roles.Select(r => r.MovieId).Distinct().Count(),
            ElsoFilmEve = a.Roles.Min(r => r.Movie.Published.Year),
            LegutobbiFilmEve = a.Roles.Max(r => r.Movie.Published.Year),
            AtlagosErtekeles = a.Roles.Select(r => new { r.MovieId, Rating = r.Movie.Rating })
                .Distinct().Average(x => x.Rating)
        })
            .OrderByDescending(x => x.FilmekSzama).ThenBy(x => x.SzineszNeve).Take(5).ToList();
        Print("6. Legfoglalkoztatottabb színészek", result,
            x => $"{x.SzineszNeve} | {x.FilmekSzama} | {x.ElsoFilmEve} | {x.LegutobbiFilmEve} | {x.AtlagosErtekeles:0.00}");
    }

    private static void QueryActorPairsInCommon(MovieDbContext db)
    {
        var result = db.Movies.AsNoTracking().SelectMany(m => m.Roles.SelectMany(r1 => m.Roles
            .Where(r2 => r1.ActorId < r2.ActorId)
            .Select(r2 => new { Elso = r1.Actor, Masodik = r2.Actor, FilmId = m.Id })))
            .GroupBy(x => new
            {
                ElsoId = x.Elso.Id,
                MasodikId = x.Masodik.Id,
                ElsoNev = x.Elso.Name,
                MasodikNev = x.Masodik.Name
            })
            .Where(g => g.Select(x => x.FilmId).Distinct().Count() >= 2)
            .Select(g => new
            {
                ElsoSzineszNeve = g.Key.ElsoNev,
                MasodikSzineszNeve = g.Key.MasodikNev,
                KozosFilmekSzama = g.Select(x => x.FilmId).Distinct().Count()
            }).OrderByDescending(x => x.KozosFilmekSzama).ToList();
        Print("7. Közösen szereplő színészpárok", result,
            x => $"{x.ElsoSzineszNeve} – {x.MasodikSzineszNeve} | {x.KozosFilmekSzama}");
    }

    private static void QueryDirectorsMostSuccessfulMovie(MovieDbContext db)
    {
        var result = db.Movies.AsNoTracking().Include(m => m.Director).Include(m => m.Roles)
            .AsEnumerable().GroupBy(m => new { m.DirectorId, RendezoNeve = m.Director.Name })
            .Select(g => g.OrderByDescending(m => m.Rating).ThenByDescending(m => m.Income)
                .Select(m => new
                {
                    g.Key.RendezoNeve,
                    FilmCime = m.Title,
                    Ertekeles = m.Rating,
                    Bevetel = m.Income,
                    SzereplokSzama = m.Roles.Count
                }).First())
            .OrderByDescending(x => x.Ertekeles).ToList();
        Print("8. Rendezők legsikeresebb filmje", result,
            x => $"{x.RendezoNeve} | {x.FilmCime} | {x.Ertekeles:0.0} | {x.Bevetel:0.0} | {x.SzereplokSzama}");
    }

    private static void Print<T>(string title, IEnumerable<T> items, Func<T, string> format)
    {
        Console.WriteLine($"\n=== {title} ===");
        foreach (var item in items) Console.WriteLine(format(item));
    }
}
