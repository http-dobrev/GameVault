using Logic.Entities;
using Logic.Enums;
using Logic.Interfaces;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;

namespace Logic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public void Register(RegisterRequest request)
        {
            // Basic defensive checks (logic-level)
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Invalid registration data.");
            }

            // Normalize input
            var email = request.Email.Trim().ToLowerInvariant();
            var username = request.Username.Trim();

            // Check uniqueness
            if (_userRepository.ExistsByEmail(email))
                throw new ArgumentException("Email is already in use.");

            if (_userRepository.ExistsByUsername(username))
                throw new ArgumentException("Username is already in use.");

            // Hash password
            var passwordHash = _passwordHasher.HashPassword(request.Password);

            // Create user entity
            var newUser = new User
            {
                Email = email,
                Username = username,
                PasswordHash = passwordHash,
                Role = UserRole.User, // Default role
                CreatedAt = DateTime.UtcNow,
                IsArchived = true
            };

            // Save to repository
            _userRepository.CreateUser(newUser);
        }

        public User Login(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.EmailOrUsername) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Invalid login data.");
            }

            User? user;

            // Decide how to fetch user
            if (request.EmailOrUsername.Contains("@"))
            {
                user = _userRepository.GetByEmail(
                    request.EmailOrUsername.Trim().ToLowerInvariant());
            }
            else
            {
                user = _userRepository.GetByUsername(
                    request.EmailOrUsername.Trim());
            }

            // Same error whether user exists or not (security)
            if (user == null)
                throw new ArgumentException("Invalid Credentials");

            // Verify password
            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                throw new ArgumentException("Invalid Credentials");

            // Optional: check if active
            if (!user.IsArchived)
                throw new ArgumentException("User is archived");

            return user;
        }
    }
}
