
using Logic.Dtos;
using Logic.Interfaces;

namespace UnitTests.FakeRepo
{
    public class FakeGameRepository : IGameRepository
    {
        private readonly List<GameDto> _games = new();
        private int _nextId = 1;

        public FakeGameRepository(IEnumerable<GameDto>? seed = null)
        {
            if (seed != null)
            {
                _games.AddRange(seed);
                if (_games.Any())
                    _nextId = _games.Max(g => g.Id) + 1;
            }
        }

        public IEnumerable<GameDto> GetAllGames()
        {
            return _games.Select(Clone).ToList();
        }

        public GameDto? GetGame(int id)
        {
            var found = _games.FirstOrDefault(g => g.Id == id);
            return found == null ? null : Clone(found);
        }
        public bool GameExists(string title)
        {
            return _games.Any(g => string.Equals(g.Title, title, StringComparison.OrdinalIgnoreCase));
        }

        public void CreateGame(GameDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var toStore = Clone(dto);
            toStore.Id = _nextId++;
            _games.Add(toStore);
        }

        public void UpdateGame(GameDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var existing = _games.FirstOrDefault(g => g.Id == dto.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Game with id {dto.Id} not found.");

            // copy fields (adjust to your real DTO properties)
            existing.Title = dto.Title;
            existing.GenreId = dto.GenreId;
            existing.DeveloperId = dto.DeveloperId;
            existing.PublisherId = dto.PublisherId;
            existing.ReleaseDate = dto.ReleaseDate;
            existing.Price = dto.Price;
            existing.PegiAge = dto.PegiAge;
            existing.Description = dto.Description;
            existing.CoverImageUrl = dto.CoverImageUrl;
            existing.UpdatedAt = dto.UpdatedAt;
        }

        public void ArchiveGame(int id)
        {
            var existing = _games.FirstOrDefault(g => g.Id == id);
            if (existing == null)
                throw new KeyNotFoundException($"Game with id {id} not found.");

            existing.IsArchived = true; // your DTO must have this; if not, adapt to your design
        }

        private static GameDto Clone(GameDto g)
        {
            return new GameDto
            {
                Id = g.Id,
                Title = g.Title,
                GenreId = g.GenreId,
                DeveloperId = g.DeveloperId,
                PublisherId = g.PublisherId,
                ReleaseDate = g.ReleaseDate,
                Price = g.Price,
                PegiAge = g.PegiAge,
                Description = g.Description,
                CoverImageUrl = g.CoverImageUrl,
                CreatedAt = g.CreatedAt,
                UpdatedAt = g.UpdatedAt,
                IsArchived = g.IsArchived
            };
        }
    }
}
