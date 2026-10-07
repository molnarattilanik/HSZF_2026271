using Microsoft.EntityFrameworkCore;

namespace EF_LINQ;

public class MovieDbContext(DbContextOptions<MovieDbContext> options) : DbContext(options)
{
    public DbSet<Director> Directors => Set<Director>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Actor> Actors => Set<Actor>();
    public DbSet<Role> Roles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Director>().HasKey(d => d.Id);
        modelBuilder.Entity<Movie>().HasKey(m => m.Id);
        modelBuilder.Entity<Actor>().HasKey(a => a.Id);
        modelBuilder.Entity<Role>().HasKey(r => r.Id);

        modelBuilder.Entity<Director>()
            .HasMany(d => d.Movies).WithOne(m => m.Director)
            .HasForeignKey(m => m.DirectorId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Roles).WithOne(r => r.Movie)
            .HasForeignKey(r => r.MovieId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Actor>()
            .HasMany(a => a.Roles).WithOne(r => r.Actor)
            .HasForeignKey(r => r.ActorId).OnDelete(DeleteBehavior.Cascade);
    }
}
