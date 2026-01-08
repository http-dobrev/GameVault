using Logic.Dtos;
using Logic.Entities;
using Logic.Enums;
using Logic.Interfaces;
using Logic.Mappers;

namespace Logic.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IUserGameRepository _userGameRepository;

        public GameService(IGameRepository gameRepository, IUserGameRepository userGameRepository)
        {
            _gameRepository = gameRepository;
            _userGameRepository = userGameRepository;
        }

        public IEnumerable<Game> GetAllGames()
        {
            var dtos = _gameRepository.GetAllGames();
            var games = new List<Game>();

            foreach (var dto in dtos)
            {
                Game game = GameMapper.ToEntity(dto);
                games.Add(game);
            }

            return games;
        }

        public Game? GetGame(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than 0.");

            var dto = _gameRepository.GetGame(id);
            if (dto == null)
                throw new KeyNotFoundException($"Game with id {id} was not found.");

            return GameMapper.ToEntity(dto);
        }

        public void CreateGame(Game game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game), "Game cannot be null.");

            if (_gameRepository.GameExists(game.Title))
                throw new ArgumentException($"A game with the title '{game.Title}' already exists.", nameof(game.Title));

            var errors = ValidateGame(game);
            if (errors.Any())
            {
                throw new ArgumentException("Game validation failed: " + string.Join("; ", errors));
            }

            game.CreatedAt = DateTime.UtcNow;
            game.UpdatedAt = DateTime.UtcNow;

            _gameRepository.CreateGame(GameMapper.ToDto(game));
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

            var dto = _gameRepository.GetGame(game.Id);
            if (dto == null)
                throw new KeyNotFoundException($"Game with id {game.Id} was not found.");

            _gameRepository.UpdateGame(GameMapper.ToDto(game));
        }

        public void ArchiveGame(Game game)
        {
            if (game.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(game.Id), "Id must be greater than 0.");

            var existing = _gameRepository.GetGame(game.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Game with id {game.Id} was not found.");


            _gameRepository.ArchiveGame(existing.Id);
        }

        /// <summary>
        /// Validates business rules for a Game entity.
        /// Returns a list of error messages. If empty, the Game is valid.
        /// </summary>
        public static List<string> ValidateGame(Game game)
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
            var minReleaseDate = new DateTime(1970, 1, 1);
            var maxReleaseDate = DateTime.UtcNow.AddYears(2);

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
