using InventoryManagementSystem.Models;
using InventoryManagementSystem.DTOs;

namespace InventoryManagementSystem.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> CreateUserAsync(UserCreateDto dto);
        Task<User> UpdateUserAsync(Guid id, UserUpdateDto dto);
        Task<bool> DeleteUserAsync(Guid id);
    }
}