using InventoryManagementSystem.DTOs;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Repositories;

namespace InventoryManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        public UserService(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync() =>
            await _userRepo.GetAllAsync();
        public async Task<User> GetUserByIdAsync(Guid id) =>
            await _userRepo.GetByIdAsync(id);
        public async Task<User> CreateUserAsync(UserCreateDto dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _userRepo.AddAsync(user);
            return user;
        }
        public async Task<User> UpdateUserAsync(Guid id, UserUpdateDto dto)
        {
            var existingUser = await _userRepo.GetByIdAsync(id);
            if (existingUser == null)
                return null;
            existingUser.Username = dto.Username;
            existingUser.Email = dto.Email;
            existingUser.UpdatedAt = DateTime.UtcNow;
            await _userRepo.UpdateAsync(existingUser);
            return existingUser;
        }
        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var existingUser = await _userRepo.GetByIdAsync(id);
            if (existingUser == null)
                return false;
            await _userRepo.DeleteAsync(id);
            return true;
        }
    }
}