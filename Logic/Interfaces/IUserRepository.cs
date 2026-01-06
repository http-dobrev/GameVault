using Logic.Dtos;

namespace Logic.Interfaces
{
    public interface IUserRepository
    {

        bool ExistsByEmail(string email);
        bool ExistsByUsername(string username);

        UserDto? GetByEmail(string email);
        UserDto? GetByUsername(string username);

        int CreateUser(UserDto user);
    }
}
