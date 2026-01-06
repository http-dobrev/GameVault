using Logic.Entities;
using Logic.Interfaces;
using Logic.Mappers;

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
            var publisherDtos = _publisherRepository.GetAllPublishers();
            var publishers = new List<Publisher>();
            foreach (var dto in publisherDtos)
            {
                publishers.Add(PublisherMapper.ToEntity(dto));
            }
            return publishers;
        }
    }
}
