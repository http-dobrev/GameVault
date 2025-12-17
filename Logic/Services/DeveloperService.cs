using Logic.Entities;
using Logic.Interfaces;

namespace Logic.Services
{
    public class DeveloperService : IDeveloperService
    {
        private readonly IDeveloperRepository _developerRepository;

        public DeveloperService(IDeveloperRepository developerRepository)
        {
            _developerRepository = developerRepository;
        }

        public IEnumerable<Developer> GetAllDevelopers()
        {
            return _developerRepository.GetAllDevelopers();
        }
    }
}
