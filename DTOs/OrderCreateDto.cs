// DTOs/OrderCreateDto.cs
using System;
using System.Collections.Generic;

namespace InventoryManagementSystem.DTOs
{
    public class OrderCreateDto
    {
        public Guid UserId { get; set; }
        public List<OrderItemCreateDto> OrderItems { get; set; }
        public string Status { get; set; }
    }
}