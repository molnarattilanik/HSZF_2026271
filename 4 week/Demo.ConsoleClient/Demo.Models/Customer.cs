namespace Demo.Models;

public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
}
