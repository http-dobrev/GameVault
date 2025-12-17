using Data.Dtos;
using Logic.Entities;

namespace Data.Mappers
{
    public static class DeveloperDataMapper
    {
        public static Developer ToEntity(DeveloperDto dto)
        {
            return new Developer
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        public static DeveloperDto ToDto(Developer entity)
        {
            return new DeveloperDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
