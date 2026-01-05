using Logic.Entities;

namespace Logic.Interfaces
{
    public interface IAuthService
    {
        void Register(RegisterRequest request);
        User Login(LoginRequest request);
    }
}
