using Domain.Gebruikers.Models;

namespace Domain.Common.Interfaces.Repository
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User? GetUserByEmail(string email);
        User GetUserOnId(int id);
        void AddUser(User user);
        bool VerwijderUser(int userId);
    }
}
