using Logic.Entities;
using Logic.Enums;
using UI.Models;

namespace UI.Mappers
{
    public static class UserGameModelMapper
    {
        public static LibraryListItemVM ToLibraryListItemVM(UserGame userGame)
        {
            return new LibraryListItemVM
            {
                Title = userGame.Game.Title,
                GenreName = userGame.Game.GenreName,
                Status = userGame.Status.ToString(),
                Platform = userGame.Platform.ToString(),
                PricePaid = userGame.PricePaid,
                OwnedSince = userGame.PurchacedAt,
                AddedAt = userGame.AddedAt,
                HoursPlayed = userGame.HoursPlayed,
                Notes = userGame.Notes,
                CoverImageURL = userGame.Game.CoverImageUrl
            };
        }
        public static UserGame ToUserGame(UserGameFormViewModel libraryCreateVM)
        {
            return new UserGame
            {
                UserId = libraryCreateVM.UserId,
                GameId = libraryCreateVM.GameId,
                AddedAt = DateTime.Now
            };
        }
    }
}
