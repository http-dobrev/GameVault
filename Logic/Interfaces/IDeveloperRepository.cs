using Logic.Entities;

namespace Logic.Interfaces
{
    public interface IDeveloperRepository
    {
        IEnumerable<Developer> GetAllDevelopers();
    }
}
