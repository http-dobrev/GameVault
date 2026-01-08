using Logic.Entities;
using Logic.Enums;
using Logic.Services;

namespace UnitTests.Services
{
    [TestClass]
    public class GameServiceTests
    {
        private static Game CreateValidGame()
        {
            return new Game
            {
                Id = 1,
                Title = "Elden Ring",
                GenreId = 1,
                DeveloperId = 1,
                PublisherId = 1,
                ReleaseDate = new DateTime(2022, 2, 25),
                Price = 59.99m,
                PegiAge = PegiAge.Pegi18, // adjust if your enum names differ
                Description = "A valid description.",
                CoverImageUrl = "https://example.com/cover.jpg"
            };
        }

        [TestMethod]
        public void ValidateGame_ValidGame_ReturnsNoErrors()
        {
            // Arrange
            var game = CreateValidGame();

            // Act
            var errors = GameService.ValidateGame(game);

            // Assert
            Assert.AreEqual(0, errors.Count, string.Join(" | ", errors));
        }

        [TestMethod]
        public void ValidateGame_EmptyTitle_ReturnsTitleRequired()
        {
            var game = CreateValidGame();
            game.Title = "   ";

            var errors = GameService.ValidateGame(game);

            CollectionAssert.Contains(errors, "Title is required.");
        }

        [TestMethod]
        public void ValidateGame_GenreIdZero_ReturnsGenreIdError()
        {
            var game = CreateValidGame();
            game.GenreId = 0;

            var errors = GameService.ValidateGame(game);

            CollectionAssert.Contains(errors, "GenreId must be greater than 0.");
        }

        [TestMethod]
        public void ValidateGame_DeveloperIdZero_ReturnsDeveloperIdError()
        {
            var game = CreateValidGame();
            game.DeveloperId = 0;

            var errors = GameService.ValidateGame(game);

            CollectionAssert.Contains(errors, "DeveloperId must be greater than 0.");
        }

        [TestMethod]
        public void ValidateGame_PublisherIdZero_ReturnsPublisherIdError()
        {
            var game = CreateValidGame();
            game.PublisherId = 0;

            var errors = GameService.ValidateGame(game);

            CollectionAssert.Contains(errors, "PublisherId must be greater than 0.");
        }

        [TestMethod]
        public void ValidateGame_ReleaseDateTooEarly_ReturnsReleaseDateInvalid()
        {
            var game = CreateValidGame();
            game.ReleaseDate = new DateTime(1969, 12, 31);

            var errors = GameService.ValidateGame(game);

            CollectionAssert.Contains(errors, "ReleaseDate is invalid.");
        }

        [TestMethod]
        public void ValidateGame_ReleaseDateTooFarInFuture_ReturnsReleaseDateInvalid()
        {
            var game = CreateValidGame();
            game.ReleaseDate = DateTime.UtcNow.AddYears(3);

            var errors = GameService.ValidateGame(game);

            CollectionAssert.Contains(errors, "ReleaseDate is invalid.");
        }

        [TestMethod]
        public void ValidateGame_NegativePrice_ReturnsPriceError()
        {
            var game = CreateValidGame();
            game.Price = -1;

            var errors = GameService.ValidateGame(game);

            CollectionAssert.Contains(errors, "Price cannot be negative.");
        }

        [TestMethod]
        public void ValidateGame_InvalidPegiAge_ReturnsInvalidPegi()
        {
            var game = CreateValidGame();
            game.PegiAge = (PegiAge)999;

            var errors = GameService.ValidateGame(game);

            CollectionAssert.Contains(errors, "Invalid PEGI age rating.");
        }

        [TestMethod]
        public void ValidateGame_EmptyDescription_ReturnsDescriptionRequired()
        {
            var game = CreateValidGame();
            game.Description = "";

            var errors = GameService.ValidateGame(game);

            CollectionAssert.Contains(errors, "Description is required.");
        }

        [TestMethod]
        public void ValidateGame_EmptyCoverUrl_ReturnsCoverRequired()
        {
            var game = CreateValidGame();
            game.CoverImageUrl = "   ";

            var errors = GameService.ValidateGame(game);

            CollectionAssert.Contains(errors, "CoverImageUrl is required.");
        }

        [TestMethod]
        public void ValidateGame_NonAbsoluteCoverUrl_ReturnsCoverUrlMustBeAbsolute()
        {
            var game = CreateValidGame();
            game.CoverImageUrl = "/images/cover.jpg"; // relative

            var errors = GameService.ValidateGame(game);

            CollectionAssert.Contains(errors, "CoverImageUrl must be a valid absolute URL.");
        }
    }
}
