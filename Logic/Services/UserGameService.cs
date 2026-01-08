using Logic.Entities;
using Logic.Enums;
using Logic.Interfaces;
using Logic.Mappers;

namespace Logic.Services
{
    public class UserGameService : IUserGameService
    {
        private readonly IUserGameRepository _userGameRepository;

        public UserGameService(IUserGameRepository userGameRepository)
        {
            _userGameRepository = userGameRepository;
        }

        public IEnumerable<UserGame> GetAllUserGames(int userId)
        {
            var userGamesDto = _userGameRepository.GetAllUserGames(userId);
            var userGames = userGamesDto.Select(ug => UserGameMapper.ToEntity(ug));
            return userGames;
        }

        public UserGame? GetUserGame(int userId, int gameId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), "UserId must be greater than 0.");
            if (gameId <= 0)
                throw new ArgumentOutOfRangeException(nameof(gameId), "GameId must be greater than 0.");
            var foundUserGame = _userGameRepository.GetUserGame(userId, gameId);
            if (foundUserGame == null)
                throw new KeyNotFoundException($"UserGame with UserId {userId} and GameId {gameId} was not found.");
            return UserGameMapper.ToEntity(foundUserGame);
        }

        public bool GameExistsInUserLibrary(int userId, int gameId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), "UserId must be greater than 0.");
            if (gameId <= 0)
                throw new ArgumentOutOfRangeException(nameof(gameId), "GameId must be greater than 0.");
            return _userGameRepository.UserGameExists(userId, gameId);
        }

        public void CreateUserGame(UserGame userGame)
        {
            if (userGame == null)
                throw new ArgumentNullException(nameof(userGame), "UserGame cannot be null.");
            if (_userGameRepository.UserGameExists(userGame.UserId, userGame.GameId))
                throw new ArgumentException($"UserGame with UserId {userGame.UserId} and GameId {userGame.GameId} already exists.");

            userGame.PurchacedAt = DateTime.Now;
            var errors = ValidateUserGame(userGame);
            if (errors.Any())
                throw new ArgumentException("UserGame validation failed: " + string.Join("; ", errors));

            _userGameRepository.CreateUserGame(UserGameMapper.ToDto(userGame));
        }

        public void UpdateUserGame(UserGame userGame)
        {
            if (userGame == null)
                throw new ArgumentNullException(nameof(userGame), "UserGame cannot be null.");

            if (!_userGameRepository.UserGameExists(userGame.UserId, userGame.GameId))
                throw new KeyNotFoundException($"UserGame with UserId {userGame.UserId} and GameId {userGame.GameId} does not exist.");

            var errors = ValidateUserGame(userGame);

            if (errors.Any())
                throw new ArgumentException("UserGame validation failed: " + string.Join("; ", errors));

            var existingUserGame = _userGameRepository.GetUserGame(userGame.UserId, userGame.GameId);

            if (existingUserGame == null)
                throw new KeyNotFoundException($"UserGame with UserId {userGame.UserId} and GameId {userGame.GameId} was not found.");

            _userGameRepository.UpdateUserGame(UserGameMapper.ToDto(userGame));
        }

        public void DeleteUserGame(UserGame userGame)
        {
            if (userGame == null)
                throw new ArgumentNullException(nameof(userGame), "UserGame cannot be null.");

            if (!_userGameRepository.UserGameExists(userGame.UserId, userGame.GameId))
                throw new KeyNotFoundException($"UserGame with UserId {userGame.UserId} and GameId {userGame.GameId} does not exist.");
            _userGameRepository.DeleteUserGame(UserGameMapper.ToDto(userGame));
        }

        public static List<string> ValidateUserGame(UserGame userGame)
        {
            var errors = new List<string>();
            if (userGame.UserId <= 0)
                errors.Add("UserId must be greater than 0.");
            if (userGame.GameId <= 0)
                errors.Add("GameId must be greater than 0.");
            if (!Enum.IsDefined(typeof(UserGameStatus), userGame.Status))
                errors.Add("Invalid UserGameStatus status.");
            if (!Enum.IsDefined(typeof(UserGamePlatform), userGame.Platform))
                errors.Add("Invalid UserGamePlatform status.");
            if (userGame.PricePaid < 0)
                errors.Add("PricePaid cannot be negative.");
            var minReleaseDate = new DateTime(1970, 1, 1);
            if (userGame.PurchacedAt <= minReleaseDate)
                errors.Add($"PurchacedAt cannot be earlier than {minReleaseDate.ToShortDateString()}.");
            if (userGame.PurchacedAt >= DateTime.Now)
                errors.Add("PurchacedAt cannot be in the future.");
            if (userGame.HoursPlayed < 0)
                errors.Add("HoursPlayed cannot be negative.");
            if (userGame.Notes != null && userGame.Notes.Length > 512)
                errors.Add("Notes cannot exceed 512 characters.");

            return errors;
        }
    }
}
