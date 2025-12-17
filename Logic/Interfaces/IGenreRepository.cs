using Logic.Entities;

namespace Logic.Interfaces
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetAllGenres();
    }
}
