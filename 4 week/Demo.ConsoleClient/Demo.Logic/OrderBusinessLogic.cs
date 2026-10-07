using Demo.ConsoleClient.Repositories;
using Demo.Models;

namespace Demo.Logic;

public interface IOrderBusinessLogic
{
    IReadOnlyList<Order> GetOrders();
    Order? GetOrder(int id);
    void CreateOrder(Order order);
    void UpdateOrder(Order order);
    void DeleteOrder(int id);
    IReadOnlyList<CustomerSpendingSummary> GetCustomerSpending();
}

public sealed class OrderBusinessLogic : IOrderBusinessLogic
{

    private readonly IRepository<Customer> customerRepository;
    private readonly IRepository<Order> orderRepository;

    public OrderBusinessLogic(
        IRepository<Customer> customerRepository,
        IRepository<Order> orderRepository)
    {
        this.customerRepository = customerRepository;
        this.orderRepository = orderRepository;
    }

    public IReadOnlyList<Order> GetOrders()
    {
        return orderRepository.ReadAll()
            .OrderBy(order => order.OrderDate)
            .ToList();
    }

    public Order? GetOrder(int id)
    {
        return orderRepository.Read(id);
    }

    public void CreateOrder(Order order)
    {
        ValidateOrder(order);
        orderRepository.Create(order);
    }

    public void UpdateOrder(Order order)
    {
        ValidateOrder(order);

        if (orderRepository.Read(order.OrderId) is null)
        {
            throw new KeyNotFoundException($"A rendelés #{order.OrderId} nem található.");
        }

        orderRepository.Update(order);
    }

    public void DeleteOrder(int id)
    {
        orderRepository.Delete(id);
    }

    public IReadOnlyList<CustomerSpendingSummary> GetCustomerSpending()
    {
        return customerRepository.ReadAll()
            .AsEnumerable()
            .Select(customer => new CustomerSpendingSummary(
                customer.Name,
                customer.Orders.Sum(order => order.TotalAmount)))
            .OrderByDescending(row => row.TotalAmount)
            .ThenBy(row => row.CustomerName)
            .ToList();
    }

    private static void ValidateOrder(Order order)
    {
        if (order.CustomerId <= 0)
        {
            throw new ArgumentException("A rendeléshez érvényes vásárló szükséges.", nameof(order));
        }

        if (order.TotalAmount < 0)
        {
            throw new ArgumentException("A rendelés összege nem lehet negatív.", nameof(order));
        }
    }
}
