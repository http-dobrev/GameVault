using Data.Dtos;
using Logic.Entities;
using Logic.Enums;

namespace Data.Mappers
{
    public static class UserDataMapper
    {
        public static User UserDtoToEntity(UserDto dto)
        {
            return new User
            {
                Id = dto.Id,
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                Role = (UserRole)dto.Role,
                CreatedAt = dto.CreatedAt,
                IsArchived = dto.IsArchived
            };
        }
        public static UserDto UserEntityToDto(User entity)
        {
            return new UserDto
            {
                Id = entity.Id,
                Username = entity.Username,
                Email = entity.Email,
                PasswordHash = entity.PasswordHash,
                Role = (int)entity.Role,
                CreatedAt = entity.CreatedAt,
                IsArchived = entity.IsArchived
            };
        }
    }
}
