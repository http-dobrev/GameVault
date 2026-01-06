using Logic.Entities;

namespace Logic.Interfaces
{
    public interface IAuthService
    {
        void Register(UserRegisterRequest request);
        User Login(LoginRequest request);
    }
}
