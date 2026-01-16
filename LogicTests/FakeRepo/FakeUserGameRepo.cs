using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Dtos;
using Logic.Interfaces;

namespace LogicTests.Fakes
{
    public class FakeUserGameRepository : IUserGameRepository
    {
        private readonly List<UserGameDto> _store = new();

        public FakeUserGameRepository(IEnumerable<UserGameDto>? seed = null)
        {
            if (seed != null) _store.AddRange(seed.Select(Clone));
        }

        public IEnumerable<UserGameDto> GetAllUserGames(int userId)
        {
            return _store
                .Where(x => x.UserId == userId)
                .Select(Clone)
                .ToList();
        }

        public UserGameDto? GetUserGame(int userId, int gameId)
        {
            var found = _store.FirstOrDefault(x => x.UserId == userId && x.GameId == gameId);
            return found == null ? null : Clone(found);
        }

        public bool UserGameExists(int userId, int gameId)
        {
            return _store.Any(x => x.UserId == userId && x.GameId == gameId);
        }

        public void CreateUserGame(UserGameDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            if (UserGameExists(dto.UserId, dto.GameId))
                throw new InvalidOperationException("Duplicate key in fake repo.");

            _store.Add(Clone(dto));
        }

        public void UpdateUserGame(UserGameDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var existing = _store.FirstOrDefault(x => x.UserId == dto.UserId && x.GameId == dto.GameId);
            if (existing == null) throw new KeyNotFoundException("UserGame not found in fake repo.");

            // Copy fields (adjust to your real DTO fields)
            existing.Status = dto.Status;
            existing.Platform = dto.Platform;
            existing.PricePaid = dto.PricePaid;
            existing.PurchacedAt = dto.PurchacedAt;
            existing.HoursPlayed = dto.HoursPlayed;
            existing.Notes = dto.Notes;
        }

        public void DeleteUserGame(int userId, int gameId)
        {
            var existing = _store.FirstOrDefault(x => x.UserId == userId && x.GameId == gameId);
            if (existing == null) throw new KeyNotFoundException("UserGame not found in fake repo.");

            _store.Remove(existing);
        }

        private static UserGameDto Clone(UserGameDto x)
        {
            return new UserGameDto
            {
                UserId = x.UserId,
                GameId = x.GameId,
                Status = x.Status,
                Platform = x.Platform,
                PricePaid = x.PricePaid,
                PurchacedAt = x.PurchacedAt,
                HoursPlayed = x.HoursPlayed,
                Notes = x.Notes
            };
        }
    }
}
