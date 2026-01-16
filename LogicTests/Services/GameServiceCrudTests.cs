using Logic.Dtos;
using Logic.Entities;
using Logic.Enums;
using Logic.Services;
using UnitTests.FakeRepo;

namespace LogicTests.Services
{
    [TestClass]
    public class GameServiceCrudTests
    {
        private FakeGameRepository _gameRepo = null!;
        private GameService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _gameRepo = new FakeGameRepository(seed: new[]
            {
                new GameDto { Id = 1, Title = "Seed Game", GenreId = 1, DeveloperId = 1, PublisherId = 1,
                             ReleaseDate = new DateTime(2020,1,1), Price = 10m, PegiAge = (int)PegiAge.Pegi18,
                             Description = "desc", CoverImageUrl = "https://example.com/a.jpg", IsArchived = false }
            });

            _service = new GameService(_gameRepo);
        }

        private static Game CreateValidGame(string title = "New Game")
        {
            return new Game
            {
                Title = title,
                GenreId = 1,
                DeveloperId = 1,
                PublisherId = 1,
                ReleaseDate = new DateTime(2021, 1, 1),
                Price = 20m,
                PegiAge = PegiAge.Pegi18,
                Description = "Valid desc",
                CoverImageUrl = "https://example.com/img.jpg"
            };
        }

        [TestMethod]
        public void GetAllGames_ReturnsSeededGames()
        {
            var games = _service.GetAllGames().ToList();
            Assert.AreEqual(1, games.Count);
            Assert.AreEqual("Seed Game", games[0].Title);
        }

        [TestMethod]
        public void CreateGame_Valid_AddsGameToRepo()
        {
            _service.CreateGame(CreateValidGame("Created"));

            var all = _gameRepo.GetAllGames().ToList();
            Assert.AreEqual(2, all.Count);
            Assert.IsTrue(all.Any(g => g.Title == "Created"));
        }

        [TestMethod]
        public void CreateGame_DuplicateTitle_Throws()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                _service.CreateGame(CreateValidGame("Seed Game")));
        }

        [TestMethod]
        public void GetGame_Existing_ReturnsGame()
        {
            var game = _service.GetGame(1);
            Assert.IsNotNull(game);
            Assert.AreEqual("Seed Game", game!.Title);
        }

        [TestMethod]
        public void GetGame_NotFound_Throws()
        {
            Assert.ThrowsException<KeyNotFoundException>(() => _service.GetGame(999));
        }

        [TestMethod]
        public void ArchiveGame_Existing_SetsArchivedFlag()
        {
            _service.ArchiveGame(new Game { Id = 1 });

            var dto = _gameRepo.GetGame(1);
            Assert.IsNotNull(dto);
            Assert.IsTrue(dto!.IsArchived);
        }

        [TestMethod]
        public void UpdateGame_Existing_UpdatesStoredDto()
        {
            var updated = CreateValidGame("Updated Title");
            updated.Id = 1;

            _service.UpdateGame(updated);

            var dto = _gameRepo.GetGame(1);
            Assert.AreEqual("Updated Title", dto!.Title);
        }
    }
}
