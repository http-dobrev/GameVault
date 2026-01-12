using Logic.Dtos;
using Logic.Entities;
using Logic.Enums;
using Logic.Interfaces;
using Logic.Mappers;


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

        public void Register(UserRegisterRequest request)
        {
            // Validate input
            var validationErrors = ValidateRegistyerRequest(request);

            if (validationErrors != null) 
                throw new ArgumentException(string.Join("; ", validationErrors));
            
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
            
            var userDto = UserMapper.UserEntityToDto(newUser);
            _userRepository.CreateUser(userDto);
        }

        public User Login(LoginRequest request)
        {
            // Validate input
            var validationErrors = ValidateLoginRequest(request);
            if (validationErrors.Any()) 
                throw new ArgumentException(string.Join("; ", validationErrors));

            UserDto? userDto;

            // Decide how to fetch user
            if (request.EmailOrUsername.Contains("@"))
            {
                userDto = _userRepository.GetByEmail(
                    request.EmailOrUsername.Trim().ToLowerInvariant());
            }
            else
            {
                userDto = _userRepository.GetByUsername(
                    request.EmailOrUsername.Trim());
            }

            // Same error whether user exists or not (security)
            if (userDto == null)
                throw new ArgumentException("Invalid Credentials");

            var user = UserMapper.UserDtoToEntity(userDto);

            // Verify password
            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                throw new ArgumentException("Invalid Credentials");

            // Optional: check if active
            if (!user.IsArchived)
                throw new ArgumentException("User is archived");

            return user;
        }

        public static List<string> ValidateRegistyerRequest(UserRegisterRequest request)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(request.Email) ||
                !request.Email.Contains("@"))
            {
                errors.Add("Invalid email format.");
            }
            if (string.IsNullOrWhiteSpace(request.Username) ||
                request.Username.Length < 3)
            {
                errors.Add("Username must be at least 3 characters long.");
            }
            if (string.IsNullOrWhiteSpace(request.Password) ||
                request.Password.Length < 6)
            {
                errors.Add("Password must be at least 6 characters long.");
            }
            return errors;
        }

        public static List<string> ValidateLoginRequest(LoginRequest request)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(request.EmailOrUsername))
            {
                errors.Add("Email or Username is required.");
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                errors.Add("Password is required.");
            }
            return errors;
        }
    }
}
