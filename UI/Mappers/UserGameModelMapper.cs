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
                GameId = userGame.GameId,
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
        public static UserGameEditViewModel ToUserGameEditViewModel(UserGame userGame)
        {
            return new UserGameEditViewModel
            {
                UserId = userGame.UserId,
                GameId = userGame.GameId,
                Status = (int?)userGame.Status,
                Platform = (int?)userGame.Platform,
                PricePaid = userGame.PricePaid,
                PurchacedAt = userGame.PurchacedAt,
                AddedAt = userGame.AddedAt,
                HoursPlayed = userGame.HoursPlayed,
                Notes = userGame.Notes
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
