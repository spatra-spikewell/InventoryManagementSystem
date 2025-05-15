using InventoryManagementSystem.DTOs;

namespace InventoryManagementSystem.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(Guid id);
        Task<OrderDto> CreateOrderAsync(OrderCreateDto dto);
        Task<OrderDto> UpdateOrderAsync(Guid id, OrderUpdateDto dto);
        Task<bool> DeleteOrderAsync(Guid id);
    }
}