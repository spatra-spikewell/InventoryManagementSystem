using InventoryManagementSystem.DTOs;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Repositories;

namespace InventoryManagementSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        public ProductService(IProductRepository productRepository)
        {
            _productRepo = productRepository;
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync() =>
            await _productRepo.GetAllAsync();
        public async Task<Product> GetProductByIdAsync(Guid id) =>
            await _productRepo.GetByIdAsync(id);
        public async Task<Product> CreateProductAsync(ProductCreateDto dto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _productRepo.AddAsync(product);
            return product;
        }
        public async Task<Product> UpdateProductAsync(Guid id, ProductUpdateDto dto)
        {
            var existingProduct = await _productRepo.GetByIdAsync(id);
            if (existingProduct == null)
                return null;
            existingProduct.Name = dto.Name;
            existingProduct.Description = dto.Description;
            existingProduct.Price = dto.Price;
            existingProduct.Stock = dto.Stock;
            existingProduct.UpdatedAt = DateTime.UtcNow;
            await _productRepo.UpdateAsync(existingProduct);
            return existingProduct;
        }
        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var existingProduct = await _productRepo.GetByIdAsync(id);
            if (existingProduct == null)
                return false;
            await _productRepo.DeleteAsync(id);
            return true;
        }
    }
}
