
namespace Logic.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string passowrd, string passwordHash);
    }
}
