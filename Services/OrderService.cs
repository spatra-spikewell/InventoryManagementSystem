using InventoryManagementSystem.DTOs;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Repositories;

namespace InventoryManagementSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;
        public OrderService(IOrderRepository orderRepo, IProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
        }
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepo.GetAllAsync();
            return orders.Select(o => new OrderDto
            {
                Id           = o.Id,
                UserId       = o.UserId,
                CreatedAt    = o.CreatedAt,
                DeliveryDate = o.DeliveryDate,
                Status       = o.Status,
                TotalAmount  = (o.OrderItems?.Sum(i => i.TotalPrice)) ?? 0m,
                OrderItems   = (o.OrderItems?.Select(i => new OrderItemDto
                {
                    Id         = i.Id,
                    ProductId  = i.ProductId,
                    Quantity   = i.Quantity,
                    UnitPrice  = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()) ?? new List<OrderItemDto>()
            });
        }
        public async Task<OrderDto> GetOrderByIdAsync(Guid id)
        {
            var o = await _orderRepo.GetByIdAsync(id);
            if (o == null) return null;
            return new OrderDto
            {
                Id           = o.Id,
                UserId       = o.UserId,
                CreatedAt    = o.CreatedAt,
                DeliveryDate = o.DeliveryDate,
                Status       = o.Status,
                TotalAmount  = (o.OrderItems?.Sum(i => i.TotalPrice)) ?? 0m,
                OrderItems   = (o.OrderItems?.Select(i => new OrderItemDto
                {
                    Id         = i.Id,
                    ProductId  = i.ProductId,
                    Quantity   = i.Quantity,
                    UnitPrice  = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()) ?? new List<OrderItemDto>()
            };
        }
                public async Task<OrderDto> CreateOrderAsync(OrderCreateDto dto)
        {
            if (dto.OrderItems == null || !dto.OrderItems.Any())
                throw new ArgumentException("Order must contain at least one item");

            foreach (var it in dto.OrderItems)
            {
                var p = await _productRepo.GetByIdAsync(it.ProductId);
                if (p == null) throw new ArgumentException($"Product {it.ProductId} not found");
                if (p.Stock < it.Quantity) throw new InvalidOperationException($"Insufficient stock for product {it.ProductId}");
            }

            var items = new List<OrderItem>();
            foreach (var it in dto.OrderItems)
            {
                var p = await _productRepo.GetByIdAsync(it.ProductId);
                p.Stock -= it.Quantity;
                p.UpdatedAt = DateTime.UtcNow;
                await _productRepo.UpdateAsync(p);

                items.Add(new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = it.ProductId,
                    Quantity = it.Quantity,
                    UnitPrice = it.UnitPrice,
                    TotalPrice = it.Quantity * it.UnitPrice,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                DeliveryDate = DateTime.UtcNow.AddDays(7),
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OrderItems = items,
                TotalAmount = items.Sum(i => i.TotalPrice)
            };

            await _orderRepo.AddAsync(order);

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                CreatedAt = order.CreatedAt,
                DeliveryDate = order.DeliveryDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };
        }
        public async Task<OrderDto> UpdateOrderAsync(Guid id, OrderUpdateDto dto)
        {
            var o = await _orderRepo.GetByIdAsync(id);
            if (o == null) return null;
            o.DeliveryDate = dto.DeliveryDate;
            o.Status       = dto.Status;
            o.UpdatedAt    = DateTime.UtcNow;
            await _orderRepo.UpdateAsync(o);
            return new OrderDto
            {
                Id           = o.Id,
                UserId       = o.UserId,
                CreatedAt    = o.CreatedAt,
                DeliveryDate = o.DeliveryDate,
                Status       = o.Status,
                TotalAmount  = (o.OrderItems?.Sum(i => i.TotalPrice)) ?? 0m,
                OrderItems   = (o.OrderItems?.Select(i => new OrderItemDto
                {
                    Id         = i.Id,
                    ProductId  = i.ProductId,
                    Quantity   = i.Quantity,
                    UnitPrice  = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()) ?? new List<OrderItemDto>()
            };
        }
        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            var exists = await _orderRepo.GetByIdAsync(id);
            if (exists == null) return false;
            await _orderRepo.DeleteAsync(id);
            return true;
        }
    }
}
