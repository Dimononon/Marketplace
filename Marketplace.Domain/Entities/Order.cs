using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; }

    public Guid BuyerId { get; set; }
    public User Buyer { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}
