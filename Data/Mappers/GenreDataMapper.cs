using Data.Dtos;
using Logic.Entities;

namespace Data.Mappers
{
    public static class GenreDataMapper
    {
        public static Genre ToEntity(GenreDto dto)
        {
            return new Genre
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        public static GenreDto ToDto(Genre entity)
        {
            return new GenreDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
