using InventoryManagementSystem.Models;
using InventoryManagementSystem.DTOs;

namespace InventoryManagementSystem.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(Guid id);
        Task<Product> CreateProductAsync(ProductCreateDto dto);
        Task<Product> UpdateProductAsync(Guid id, ProductUpdateDto dto);
        Task<bool> DeleteProductAsync(Guid id);
    }
}