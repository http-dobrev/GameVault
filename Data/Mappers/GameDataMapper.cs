using Data.Dtos;
using Logic.Entities;
using Logic.Enums;

namespace Data.Mappers
{
    public static class GameDataMapper
    {
        public static Game ToEntity(GameDto dto)
        {
            return new Game
            {
                Id = dto.Id,
                Title = dto.Title,
                GenreId = dto.GenreId,
                GenreName = dto.GenreName,
                ReleaseDate = dto.ReleaseDate,
                DeveloperId = dto.DeveloperId,
                DeveloperName = dto.DeveloperName,
                PublisherId = dto.PublisherId,
                PublisherName = dto.PublisherName,
                Price = dto.Price,
                PegiAge = (PegiAge)dto.PegiAge,
                Description = dto.Description,
                CoverImageUrl = dto.CoverImageUrl,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                IsArchived = dto.IsArchived
            };
        }

        public static GameDto ToDto(Game entity)
        {
            return new GameDto
            {
                Id = entity.Id,
                Title = entity.Title,
                GenreId = entity.GenreId,
                ReleaseDate = entity.ReleaseDate,
                DeveloperId = entity.DeveloperId,
                PublisherId = entity.PublisherId,
                Price = entity.Price,
                PegiAge = (int)entity.PegiAge,
                Description = entity.Description,
                CoverImageUrl = entity.CoverImageUrl,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                IsArchived = entity.IsArchived
            };
        }

    }
}
