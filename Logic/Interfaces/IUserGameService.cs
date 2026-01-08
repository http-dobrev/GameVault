using Logic.Entities;

namespace Logic.Interfaces
{
    public interface IUserGameService
    {
        IEnumerable<UserGame> GetAllUserGames(int userId);
        UserGame? GetUserGame(int userId, int gameId);
        bool GameExistsInUserLibrary(int userId, int gameId);
        void CreateUserGame(UserGame userGame);
        void UpdateUserGame(UserGame userGame);
        void DeleteUserGame(int userId, int gameId);
    }
}
