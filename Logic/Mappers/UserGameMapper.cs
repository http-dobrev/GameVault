using Logic.Entities;
using Logic.Enums;
using Logic.Dtos;

namespace Logic.Mappers
{
    public class UserGameMapper
    {
        public static UserGame ToEntity(UserGameDto dto)
        {
            return new UserGame
            {
                UserId = dto.UserId,
                GameId = dto.GameId,
                Status = (UserGameStatus)dto.Status,
                Platform = (UserGamePlatform)dto.Platform,
                PricePaid = dto.PricePaid,
                PurchacedAt = dto.PurchacedAt,
                AddedAt = dto.AddedAt,
                HoursPlayed = dto.HoursPlayed,
                Notes = dto.Notes,
                Game = GameMapper.ToEntity(dto.Game)
            };
        }
        public static UserGameDto ToDto(UserGame entity)
        {
            return new UserGameDto
            {
                UserId = entity.UserId,
                GameId = entity.GameId,
                Status = (int)entity.Status,
                Platform = (int)entity.Platform,
                PricePaid = entity.PricePaid,
                PurchacedAt = entity.PurchacedAt,
                AddedAt = entity.AddedAt,
                HoursPlayed = entity.HoursPlayed,
                Notes = entity.Notes
            };
        }
    }
}
