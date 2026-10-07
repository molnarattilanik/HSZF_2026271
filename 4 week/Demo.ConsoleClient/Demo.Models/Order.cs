namespace Demo.Models;

public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsPaid { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
