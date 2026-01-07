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
                Title = userGame.Game.Title,
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
        public static UserGame ToUserGameFromFormViewModel(UserGameFormViewModel libraryCreateVM)
        {
            return new UserGame
            {
                UserId = libraryCreateVM.UserId,
                GameId = libraryCreateVM.GameId,
                AddedAt = DateTime.Now
            };
        }

        public static UserGame ToUserGameFromEditViewModel(UserGameEditViewModel vm)
        {
            return new UserGame
            {
                UserId = vm.UserId,
                GameId = vm.GameId,
                Status = (UserGameStatus)(vm.Status ?? 0),
                Platform = (UserGamePlatform)(vm.Platform ?? 0),
                PricePaid = vm.PricePaid ?? 0m,
                PurchacedAt = vm.PurchacedAt ?? default, // Fixes CS0266 and CS8629
                HoursPlayed = vm.HoursPlayed ?? 0,
                Notes = vm.Notes ?? string.Empty
            };
        }
    }
}
