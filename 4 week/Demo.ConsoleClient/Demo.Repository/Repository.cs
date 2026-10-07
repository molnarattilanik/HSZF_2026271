using Demo.Repository.Database;

namespace Demo.ConsoleClient.Repositories;

public interface IRepository<T> where T : class
{
    IQueryable<T> ReadAll();
    T Read(int id);
    void Create(T item);
    void Update(T item);
    void Delete(int id);
}

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected ShopContext Context { get; }

    public Repository(ShopContext context)
    {
        Context = context;
    }

    public IQueryable<T> ReadAll() => Context.Set<T>();

    public T? Read(int id) => Context.Set<T>().Find(id);

    public void Create(T item)
    {
        Context.Set<T>().Add(item);
        Context.SaveChanges();
    }

    public void Update(T item)
    {
        Context.Set<T>().Update(item);
        Context.SaveChanges();
    }

    public void Delete(int id)
    {
        var item = Read(id) ?? throw new KeyNotFoundException($"A(z) {typeof(T).Name} #{id} nem található.");
        Context.Set<T>().Remove(item);
        Context.SaveChanges();
    }
}
