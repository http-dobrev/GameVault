using Logic.Entities;

namespace Logic.Interfaces
{
    public interface IGameService
    {
        IEnumerable<Game> GetAllGames();
        Game? GetGame(int id);
        void CreateGame(Game game);
        void UpdateGame(Game game);
        void ArchiveGame(Game game);
    }
}
