using Logic.Entities;
using UI.Models;

namespace UI.Mappers
{
    public class UserAuthMapper
    {
        public static UserRegisterRequest ToRegisterRequest(UserRegisterViewModel viewModel)
        {
            return new UserRegisterRequest
            {
                Email = viewModel.Email,
                Username = viewModel.Username,
                Password = viewModel.Password
            };
        }

        public static LoginRequest ToLoginRequest(UserLoginViewModel viewModel)
        {
            return new LoginRequest
            {
                EmailOrUsername = viewModel.EmailOrUsername,
                Password = viewModel.Password
            };
        }
    }
}
