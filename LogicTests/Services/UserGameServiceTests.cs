using Logic.Entities;
using Logic.Enums;
using Logic.Services;

namespace LogicTests.Services
{
    [TestClass]
    public class UserGameValidationTests
    {
        private static UserGame CreateValidUserGame()
        {
            return new UserGame
            {
                UserId = 1,
                GameId = 1,
                Status = UserGameStatus.Playing,        // adjust enum value name if needed
                Platform = UserGamePlatform.PC,         // adjust enum value name if needed
                PricePaid = 10.00m,
                PurchacedAt = DateTime.Now.AddMinutes(-5),
                HoursPlayed = 3,
                Notes = "All good."
            };
        }

        [TestMethod]
        public void ValidateUserGame_ValidUserGame_ReturnsNoErrors()
        {
            var ug = CreateValidUserGame();

            var errors = UserGameService.ValidateUserGame(ug); // change class name if different

            Assert.AreEqual(0, errors.Count, string.Join(" | ", errors));
        }

        [TestMethod]
        public void ValidateUserGame_UserIdZero_ReturnsUserIdError()
        {
            var ug = CreateValidUserGame();
            ug.UserId = 0;

            var errors = UserGameService.ValidateUserGame(ug);

            CollectionAssert.Contains(errors, "UserId must be greater than 0.");
        }

        [TestMethod]
        public void ValidateUserGame_GameIdZero_ReturnsGameIdError()
        {
            var ug = CreateValidUserGame();
            ug.GameId = 0;

            var errors = UserGameService.ValidateUserGame(ug);

            CollectionAssert.Contains(errors, "GameId must be greater than 0.");
        }

        [TestMethod]
        public void ValidateUserGame_InvalidStatus_ReturnsInvalidStatusError()
        {
            var ug = CreateValidUserGame();
            ug.Status = (UserGameStatus)999;

            var errors = UserGameService.ValidateUserGame(ug);

            CollectionAssert.Contains(errors, "Invalid UserGameStatus status.");
        }

        [TestMethod]
        public void ValidateUserGame_InvalidPlatform_ReturnsInvalidPlatformError()
        {
            var ug = CreateValidUserGame();
            ug.Platform = (UserGamePlatform)999;

            var errors = UserGameService.ValidateUserGame(ug);

            CollectionAssert.Contains(errors, "Invalid UserGamePlatform status.");
        }

        [TestMethod]
        public void ValidateUserGame_NegativePricePaid_ReturnsPricePaidError()
        {
            var ug = CreateValidUserGame();
            ug.PricePaid = -0.01m;

            var errors = UserGameService.ValidateUserGame(ug);

            CollectionAssert.Contains(errors, "PricePaid cannot be negative.");
        }

        [TestMethod]
        public void ValidateUserGame_PurchacedAtTooEarly_ReturnsTooEarlyError()
        {
            var ug = CreateValidUserGame();
            ug.PurchacedAt = new DateTime(1970, 1, 1); // <= minReleaseDate triggers

            var errors = UserGameService.ValidateUserGame(ug);

            // Must match EXACT string your validator builds (date format depends on culture!)
            // Better approach: assert it contains the stable part.
            Assert.IsTrue(errors.Exists(e => e.StartsWith("PurchacedAt cannot be earlier than")),
                "Expected PurchacedAt earlier-than-min error, got: " + string.Join(" | ", errors));
        }

        [TestMethod]
        public void ValidateUserGame_PurchacedAtInFuture_ReturnsFutureError()
        {
            var ug = CreateValidUserGame();
            ug.PurchacedAt = DateTime.Now.AddMinutes(5);

            var errors = UserGameService.ValidateUserGame(ug);

            CollectionAssert.Contains(errors, "PurchacedAt cannot be in the future.");
        }

        [TestMethod]
        public void ValidateUserGame_NegativeHoursPlayed_ReturnsHoursPlayedError()
        {
            var ug = CreateValidUserGame();
            ug.HoursPlayed = -1;

            var errors = UserGameService.ValidateUserGame(ug);

            CollectionAssert.Contains(errors, "HoursPlayed cannot be negative.");
        }

        [TestMethod]
        public void ValidateUserGame_NotesTooLong_ReturnsNotesTooLongError()
        {
            var ug = CreateValidUserGame();
            ug.Notes = new string('a', 513);

            var errors = UserGameService.ValidateUserGame(ug);

            CollectionAssert.Contains(errors, "Notes cannot exceed 512 characters.");
        }
    }
}
