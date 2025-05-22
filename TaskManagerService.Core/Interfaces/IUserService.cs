using TaskManagerService.Core.Models;

namespace TaskManagerService.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterAsync(UserDto userDto);
        Task<TokenResponse> LoginAsync(LoginDto loginDto);
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> UpdateUserAsync(int id, UserDto userDto);
        Task DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(string email);
        Task<User> GetUserByEmailAsync(string email);
    }
}
