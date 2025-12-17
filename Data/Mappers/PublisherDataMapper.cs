using Data.Dtos;
using Logic.Entities;

namespace Data.Mappers
{
    public static class PublisherDataMapper
    {
        public static Publisher ToEntity(PublisherDto dto)
        {
            return new Publisher
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        public static PublisherDto ToDto(Publisher entity)
        {
            return new PublisherDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
