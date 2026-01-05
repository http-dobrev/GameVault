using Logic.Entities;

namespace Logic.Interfaces
{
    public interface IUserRepository
    {

        bool ExistsByEmail(string email);
        bool ExistsByUsername(string username);

        User? GetByEmail(string email);
        User? GetByUsername(string username);

        int CreateUser(User user);
    }
}
