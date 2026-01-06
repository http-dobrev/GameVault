using Logic.Entities;
using Logic.Interfaces;
using Logic.Mappers;

namespace Logic.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public IEnumerable<Genre> GetAllGenres()
        {
            var genreDtos = _genreRepository.GetAllGenres();
            var genres = new List<Genre>();
            foreach (var dto in genreDtos)
            {
                genres.Add(GenreMapper.ToEntity(dto));
            }
            return genres;
        }
    }
}
