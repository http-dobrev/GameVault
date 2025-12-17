using Logic.Entities;
using Logic.Interfaces;

namespace Logic.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublisherService(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        public IEnumerable<Publisher> GetAllPublishers()
        {
            return _publisherRepository.GetAllPublishers();
        }
    }
}
