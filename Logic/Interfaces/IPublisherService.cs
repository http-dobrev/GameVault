using Logic.Entities;

namespace Logic.Interfaces
{
    public interface IPublisherService
    {
        IEnumerable<Publisher> GetAllPublishers();
    }
}
