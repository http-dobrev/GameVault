using Logic.Dtos;
using Logic.Entities;

namespace Logic.Mappers
{
    public static class PublisherMapper
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
