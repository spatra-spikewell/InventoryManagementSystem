using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMSDbContext _context;
        public UserRepository(IMSDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.ToListAsync();
        public async Task<User> GetByIdAsync(Guid id) =>
            await _context.Users.FindAsync(id);
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}