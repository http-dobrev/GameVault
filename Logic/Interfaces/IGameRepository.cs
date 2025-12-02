using Logic.Entities;

namespace Logic.Interfaces
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetAllGames();
        Game? GetById(int id);

        void CreateGame(Game game);
        void UpdateGame(Game game);
        void DeleteGame(int id);
    }
}
