using Microsoft.EntityFrameworkCore;

namespace EF_LINQ;

public class MovieDbContext : DbContext
{
    public DbSet<Director> Directors { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Role> Roles { get; set; }

    public MovieDbContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("movies.db").UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Director>().HasKey(d => d.Id);
        modelBuilder.Entity<Movie>().HasKey(m => m.Id);
        modelBuilder.Entity<Actor>().HasKey(a => a.Id);
        modelBuilder.Entity<Role>().HasKey(r => r.Id);
        modelBuilder.Entity<Director>().HasMany(d => d.Movies).WithOne(m => m.Director)
            .HasForeignKey(m => m.DirectorId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Movie>().HasMany(m => m.Roles).WithOne(r => r.Movie)
            .HasForeignKey(r => r.MovieId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Actor>().HasMany(a => a.Roles).WithOne(r => r.Actor)
            .HasForeignKey(r => r.ActorId).OnDelete(DeleteBehavior.Cascade);

        SeedData.Configure(modelBuilder);
    }
}
