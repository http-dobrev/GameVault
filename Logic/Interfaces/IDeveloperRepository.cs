using Logic.Dtos;

namespace Logic.Interfaces
{
    public interface IDeveloperRepository
    {
        IEnumerable<DeveloperDto> GetAllDevelopers();
    }
}
