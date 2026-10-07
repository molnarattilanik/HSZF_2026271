using Demo.Models;
using Demo.Repository.Database;

namespace Demo.ConsoleClient.Repositories;

public sealed class OrderRepository : Repository<Order>, IRepository<Order>
{
    public OrderRepository(ShopContext context) : base(context)
    {
    }
}
