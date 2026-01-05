using Logic.Entities;

namespace Logic.Interfaces
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetAllGames();
        Game? GetGame(Game game);
        bool GameExists(string title);
        void CreateGame(Game game);
        void UpdateGame(Game game);
        void ArchiveGame(Game game);
    }
}
