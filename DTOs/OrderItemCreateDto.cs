using System;

namespace InventoryManagementSystem.DTOs
{
    public class OrderItemCreateDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}