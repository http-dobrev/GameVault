using Logic.Entities;
using Logic.Interfaces;
using Logic.Dtos;
using Logic.Mappers;

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
            var developerDtos = _developerRepository.GetAllDevelopers();
            var developers = new List<Developer>();
            foreach (var dto in developerDtos)
            {
                developers.Add(DeveloperMapper.ToEntity(dto));
            }
            return developers;
        }
    }
}
