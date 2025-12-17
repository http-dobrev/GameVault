using Logic.Entities;

namespace Logic.Interfaces
{
    public interface IPublisherRepository
    {
        IEnumerable<Publisher> GetAllPublishers();
    }
}
