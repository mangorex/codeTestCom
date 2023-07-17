using codeTestCom.Models;

namespace codeTestCom.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserAsyncByDni(string id);
        Task<User> UpdateUserLoyaltyAsync(User user, int loyaltyPoints);
    }
}
