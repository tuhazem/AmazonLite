using System;
using System.Collections.Generic;

namespace Amazon.Application.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }
        public IEnumerable<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
    }

    public class OrderItemDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
