using Demo.Models;
using Demo.Repository.Database;

namespace Demo.ConsoleClient.Repositories;

public sealed class CustomerRepository : Repository<Customer>, IRepository<Customer>
{
    public CustomerRepository(ShopContext context) : base(context)
    {
    }
}
