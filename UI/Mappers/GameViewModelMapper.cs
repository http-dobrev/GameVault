using Logic.Entities;
using Logic.Enums;
using UI.Models;

namespace UI.Mappers
{
    public static class GameViewModelMapper
    {
        public static GameViewModel ToViewModel(Game game)
        {
            return new GameViewModel
            {
                Id = game.Id,
                Title = game.Title,
                GenreId = game.GenreId,
                ReleaseDate = game.ReleaseDate,
                DeveloperId = game.DeveloperId,
                PublisherId = game.PublisherId,
                Price = game.Price,
                PegiAge = (int)game.PegiAge,
                Description = game.Description,
                CoverImageUrl = game.CoverImageUrl,
                CreatedAt = game.CreatedAt,
                UpdatedAt = game.UpdatedAt,
                IsArchived = game.IsArchived
            };
        }

        public static Game ToEntity(GameViewModel viewModel)
        {
            return new Game
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                GenreId = viewModel.GenreId,
                ReleaseDate = viewModel.ReleaseDate,
                DeveloperId = viewModel.DeveloperId,
                PublisherId = viewModel.PublisherId,
                Price = viewModel.Price,
                PegiAge = (PegiAge)viewModel.PegiAge,
                Description = viewModel.Description,
                CoverImageUrl = viewModel.CoverImageUrl,
                CreatedAt = viewModel.CreatedAt,
                UpdatedAt = viewModel.UpdatedAt,
                IsArchived = viewModel.IsArchived
            };
        }
    }
}
