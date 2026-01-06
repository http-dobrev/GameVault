using Logic.Dtos;

namespace Logic.Interfaces
{
    public interface IPublisherRepository
    {
        IEnumerable<PublisherDto> GetAllPublishers();
    }
}
