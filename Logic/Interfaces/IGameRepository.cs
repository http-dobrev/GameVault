using Logic.Dtos;

namespace Logic.Interfaces
{
    public interface IGameRepository
    {
        IEnumerable<GameDto> GetAllGames();
        GameDto? GetGame(int id);
        bool GameExists(string title);
        void CreateGame(GameDto game);
        void UpdateGame(GameDto game);
        void ArchiveGame(int id);
    }
}
