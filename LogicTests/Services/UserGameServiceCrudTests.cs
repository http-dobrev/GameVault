using Logic.Services;
using Logic.Entities;
using Logic.Enums;
using Logic.Dtos;
using LogicTests.Fakes;

namespace LogicTests.Services
{
    [TestClass]
    public class UserGameServiceCrudTests
    {
        private FakeUserGameRepository _repo = null!;
        private UserGameService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _repo = new FakeUserGameRepository(seed: new[]
            {
                new UserGameDto
                {
                    UserId = 1,
                    GameId = 10,
                    Status = (int)UserGameStatus.Playing,     // adjust if your DTO uses enum directly
                    Platform = (int)UserGamePlatform.PC,      // adjust if your DTO uses enum directly
                    PricePaid = 5m,
                    PurchacedAt = new DateTime(2023, 1, 1),
                    HoursPlayed = 12,
                    Notes = "seed"
                }
            });

            _service = new UserGameService(_repo);
        }

        private static UserGame CreateValidUserGame(int userId = 1, int gameId = 99)
        {
            return new UserGame
            {
                UserId = userId,
                GameId = gameId,
                Status = UserGameStatus.Playing,
                Platform = UserGamePlatform.PC,
                PricePaid = 10m,
                PurchacedAt = new DateTime(2023, 1, 1), // service overwrites this in Create anyway
                HoursPlayed = 0,
                Notes = "ok"
            };
        }

        [TestMethod]
        public void GetAllUserGames_ReturnsOnlyThatUsersGames()
        {
            var result = _service.GetAllUserGames(1).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(10, result[0].GameId);
        }

        [TestMethod]
        public void GetUserGame_InvalidUserId_Throws()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.GetUserGame(0, 10));
        }

        [TestMethod]
        public void GetUserGame_InvalidGameId_Throws()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.GetUserGame(1, 0));
        }

        [TestMethod]
        public void GetUserGame_NotFound_Throws()
        {
            Assert.ThrowsException<KeyNotFoundException>(() => _service.GetUserGame(1, 999));
        }

        [TestMethod]
        public void GetUserGame_Found_ReturnsEntity()
        {
            var ug = _service.GetUserGame(1, 10);
            Assert.IsNotNull(ug);
            Assert.AreEqual(1, ug!.UserId);
            Assert.AreEqual(10, ug.GameId);
        }

        [TestMethod]
        public void GameExistsInUserLibrary_InvalidIds_Throw()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.GameExistsInUserLibrary(0, 10));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.GameExistsInUserLibrary(1, 0));
        }

        [TestMethod]
        public void GameExistsInUserLibrary_ReturnsTrueWhenExists()
        {
            var exists = _service.GameExistsInUserLibrary(1, 10);
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void CreateUserGame_Null_Throws()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _service.CreateUserGame(null!));
        }

        [TestMethod]
        public void CreateUserGame_Duplicate_Throws()
        {
            var dup = CreateValidUserGame(1, 10);

            Assert.ThrowsException<ArgumentException>(() => _service.CreateUserGame(dup));
        }

        [TestMethod]
        public void CreateUserGame_Valid_AddsToRepo()
        {
            var ug = CreateValidUserGame(1, 77);

            _service.CreateUserGame(ug);

            Assert.IsTrue(_repo.UserGameExists(1, 77));
            var stored = _repo.GetUserGame(1, 77);
            Assert.IsNotNull(stored);

            // Don't be stupid and assert exact DateTime.Now equality.
            // Just ensure it isn't the default/min date.
            Assert.IsTrue(stored!.PurchacedAt > new DateTime(1970, 1, 1));
        }

        [TestMethod]
        public void UpdateUserGame_Null_Throws()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _service.UpdateUserGame(null!));
        }

        [TestMethod]
        public void UpdateUserGame_NotExists_Throws()
        {
            var ug = CreateValidUserGame(1, 999);

            Assert.ThrowsException<KeyNotFoundException>(() => _service.UpdateUserGame(ug));
        }

        [TestMethod]
        public void UpdateUserGame_Valid_UpdatesStoredValues()
        {
            var ug = CreateValidUserGame(1, 10);
            ug.PricePaid = 42m;
            ug.HoursPlayed = 123;
            ug.Notes = "updated";
            ug.PurchacedAt = DateTime.Now.AddDays(-1); // must not be future

            _service.UpdateUserGame(ug);

            var stored = _repo.GetUserGame(1, 10)!;
            Assert.AreEqual(42m, stored.PricePaid);
            Assert.AreEqual(123, stored.HoursPlayed);
            Assert.AreEqual("updated", stored.Notes);
        }

        [TestMethod]
        public void DeleteUserGame_InvalidIds_Throw()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.DeleteUserGame(0, 10));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.DeleteUserGame(1, 0));
        }

        [TestMethod]
        public void DeleteUserGame_NotExists_Throws()
        {
            Assert.ThrowsException<KeyNotFoundException>(() => _service.DeleteUserGame(1, 999));
        }

        [TestMethod]
        public void DeleteUserGame_Existing_RemovesFromRepo()
        {
            _service.DeleteUserGame(1, 10);

            Assert.IsFalse(_repo.UserGameExists(1, 10));
        }
    }
}
