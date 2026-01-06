using Logic.Dtos;

namespace Logic.Interfaces
{
    public interface IGenreRepository
    {
        IEnumerable<GenreDto> GetAllGenres();
    }
}
