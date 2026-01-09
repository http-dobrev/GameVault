
namespace Logic.Entities
{
    public class LoginRequest
    {
        public string EmailOrUsername { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
