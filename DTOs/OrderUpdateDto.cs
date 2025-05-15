using System;

namespace InventoryManagementSystem.DTOs
{
    public class OrderUpdateDto
    {
        public DateTime? DeliveryDate { get; set; }
        public string Status { get; set; }
    }
}