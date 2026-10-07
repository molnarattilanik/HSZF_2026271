using Demo.ConsoleClient.Repositories;
using Demo.Logic;
using Demo.Models;
using Demo.Repository.Database;

namespace Demo.ConsoleClient;

internal static class Program
{
    private static void Main()
    {
        ShopContext context = new ShopContext();

        IRepository<Customer> customerRepository = new CustomerRepository(context);
        IRepository<Order> orderRepository = new OrderRepository(context);
        IOrderBusinessLogic logic = new OrderBusinessLogic(customerRepository, orderRepository);

        RunSimpleOrderCrud(logic);

        Console.WriteLine("\nKomplex lekérdezés - vásárlónkénti összesítés:");
        foreach (var row in logic.GetCustomerSpending())
        {
            Console.WriteLine($"- {row.CustomerName}: {row.TotalAmount:N0} Ft");
        }
    }

    private static void RunSimpleOrderCrud(IOrderBusinessLogic logic)
    {
        Console.WriteLine("Egyszerű Order CRUD:");

        var firstOrder = new Order
        {
            CustomerId = 1,
            OrderDate = new DateTime(2026, 10, 1),
            TotalAmount = 15900m,
            IsPaid = false
        };
        var secondOrder = new Order
        {
            CustomerId = 2,
            OrderDate = new DateTime(2026, 10, 2),
            TotalAmount = 24900m,
            IsPaid = false
        };

        // Create
        logic.CreateOrder(firstOrder);
        logic.CreateOrder(secondOrder);
        Console.WriteLine($"Create: #{firstOrder.OrderId} és #{secondOrder.OrderId}");

        // Read - két tényleges Order visszaolvasása
        var readFirst = logic.GetOrder(firstOrder.OrderId);
        var readSecond = logic.GetOrder(secondOrder.OrderId);
        Console.WriteLine($"Read: #{readFirst?.OrderId} - {readFirst?.TotalAmount:N0} Ft");
        Console.WriteLine($"Read: #{readSecond?.OrderId} - {readSecond?.TotalAmount:N0} Ft");

        // Update
        firstOrder.TotalAmount = 17900m;
        firstOrder.IsPaid = true;
        logic.UpdateOrder(firstOrder);
        Console.WriteLine($"Update: #{firstOrder.OrderId} - {firstOrder.TotalAmount:N0} Ft, fizetve");

        // Delete
        logic.DeleteOrder(secondOrder.OrderId);
        Console.WriteLine($"Delete: #{secondOrder.OrderId}");
    }
}
