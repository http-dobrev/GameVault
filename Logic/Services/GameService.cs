using Logic.Entities;
using Logic.Enums;
using Logic.Interfaces;

namespace Logic.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public IEnumerable<Game> GetAllGames()
        {
            return _gameRepository.GetAllGames();
        }

        public Game? GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than 0.");

            var game = _gameRepository.GetById(id);

            if (game == null)
                throw new KeyNotFoundException($"Game with id {id} was not found.");

            return game;
        }

        public void CreateGame(Game game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game), "Game cannot be null.");

            var errors = ValidateGame(game);
            if (errors.Any())
            {
                // You could create a custom ValidationException; for now, keep it simple.
                var message = string.Join(Environment.NewLine, errors);
                throw new ArgumentException(message, nameof(game));
            }

            game.CreatedAt = DateTime.UtcNow;
            game.UpdatedAt = DateTime.UtcNow;

            _gameRepository.CreateGame(game);
        }

        public void UpdateGame(Game game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game), "Game cannot be null.");

            if (game.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(game.Id), "Game Id must be greater than 0.");

            var errors = ValidateGame(game);
            if (errors.Any())
            {
                var message = string.Join(Environment.NewLine, errors);
                throw new ArgumentException(message, nameof(game));
            }

            var existing = _gameRepository.GetById(game.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Game with id {game.Id} was not found.");

            _gameRepository.UpdateGame(game);
        }

        public void DeleteGame(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than 0.");

            var existing = _gameRepository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException($"Game with id {id} was not found.");

            _gameRepository.DeleteGame(id);
        }

        /// <summary>
        /// Validates business rules for a Game entity.
        /// Returns a list of error messages. If empty, the Game is valid.
        /// </summary>
        public List<string> ValidateGame(Game game)
        {
            var errors = new List<string>();

            // Adjust properties to match your Game entity
            if (string.IsNullOrWhiteSpace(game.Title))
                errors.Add("Title is required.");

            if (game.GenreId <= 0)
                errors.Add("GenreId must be greater than 0.");

            if (game.DeveloperId <= 0)
                errors.Add("DeveloperId must be greater than 0.");

            if (game.PublisherId <= 0)
                errors.Add("PublisherId must be greater than 0.");

            // Basic sanity for release date (tune as you like)
            // --- DateOnly validation ---
            var minReleaseDate = new DateOnly(1970, 1, 1);
            var maxReleaseDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(2));

            if (game.ReleaseDate < minReleaseDate || game.ReleaseDate > maxReleaseDate)
                errors.Add("ReleaseDate is invalid.");

            if (game.Price < 0)
                errors.Add("Price cannot be negative.");

            if (!Enum.IsDefined(typeof(PegiAge), game.PegiAge))
                errors.Add("Invalid PEGI age rating.");


            if (string.IsNullOrWhiteSpace(game.Description))
                errors.Add("Description is required.");

            if (string.IsNullOrWhiteSpace(game.CoverImageUrl))
            { 
                errors.Add("CoverImageUrl is required."); 
            }
            else if (!Uri.TryCreate(game.CoverImageUrl, UriKind.Absolute, out _))
            { 
                errors.Add("CoverImageUrl must be a valid absolute URL."); 
            }

            return errors;
        }
    }
}
