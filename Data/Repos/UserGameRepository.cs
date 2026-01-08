using Logic.Entities;
using Logic.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Logic.Dtos;
using Logic.Mappers;

namespace Logic.Repos
{
    public class UserGameRepository : IUserGameRepository
    {
        private readonly string _connectionString;

        public UserGameRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }

        public IEnumerable<UserGameDto> GetAllUserGames(int userId)
        {
            var userGames = new List<UserGameDto>();
            const string sql = @"SELECT
                                    ug.*, 
                                    g.Title,
                                    g.CoverImageUrl,
                                    gen.Name AS GenreName, 
                                    dev.Name AS DeveloperName, 
                                    pub.Name AS PublisherName
                                FROM UserGame ug
                                INNER JOIN Game g ON ug.GameId = g.Id
                                INNER JOIN Genre gen ON g.GenreId = gen.Id
                                INNER JOIN Developer dev ON g.DeveloperId = dev.Id
                                INNER JOIN Publisher pub ON g.PublisherId = pub.Id
                                WHERE ug.UserId = @UserId";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@UserId", userId);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var dto = new UserGameDto
                {
                    UserId = (int)reader["UserId"],
                    GameId = (int)reader["GameId"],
                    Status = (byte)reader["Status"],
                    Platform = (byte)reader["Platform"],
                    PricePaid = (decimal)reader["PricePaid"],
                    PurchacedAt = (DateTime)reader["PurchacedAt"],
                    AddedAt = (DateTime)reader["AddedAt"],
                    HoursPlayed = (int)reader["HoursPlayed"],
                    Notes = (string)reader["Notes"],
                    Game = new GameDto
                    {
                        Id = (int)reader["GameId"],
                        Title = (string)reader["Title"],
                        GenreName = (string)reader["GenreName"],
                        DeveloperName = (string)reader["DeveloperName"],
                        PublisherName = (string)reader["PublisherName"],
                        CoverImageUrl = (string)reader["CoverImageUrl"]
                    }
                };
                userGames.Add(dto);
            }
            reader.Close();
            conn.Close();
            return userGames;
        }

        public UserGameDto? GetUserGame(int userId, int gameId)
        {
            const string sql =@"SELECT 
                                    ug.*, 
                                    g.Title,
                                    gen.Name AS GenreName, 
                                    dev.Name AS DeveloperName, 
                                    pub.Name AS PublisherName
                                FROM UserGame ug
                                INNER JOIN Game g ON ug.GameId = g.Id
                                INNER JOIN Genre gen ON g.GenreId = gen.Id
                                INNER JOIN Developer dev ON g.DeveloperId = dev.Id
                                INNER JOIN Publisher pub ON g.PublisherId = pub.Id
                                WHERE ug.UserId = @UserId AND ug.GameId = GameId";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@GameId", gameId);

            conn.Open();

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                var dto = new UserGameDto
                {
                    UserId = (int)reader["UserId"],
                    GameId = (int)reader["GameId"],
                    Status = (byte)reader["Status"],
                    Platform = (byte)reader["Platform"],
                    PricePaid = (decimal)reader["PricePaid"],
                    PurchacedAt = (DateTime)reader["PurchacedAt"],
                    AddedAt = (DateTime)reader["AddedAt"],
                    HoursPlayed = (int)reader["HoursPlayed"],
                    Notes = (string)reader["Notes"],
                    Game = new GameDto
                    {
                        Id = (int)reader["GameId"],
                        Title = (string)reader["Title"],
                        GenreName = (string)reader["GenreName"],
                        DeveloperName = (string)reader["DeveloperName"],
                        PublisherName = (string)reader["PublisherName"]
                    }
                };
                reader.Close();
                conn.Close();
                return dto;
            }
            reader.Close();
            conn.Close();
            return null;
        }

        public bool UserGameExists(int userId, int gameId)
        {
            const string sql = "SELECT COUNT(1) FROM UserGame WHERE UserId = @UserId AND GameId = @GameId";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@GameId", gameId);

            conn.Open();
            var count = (int)cmd.ExecuteScalar();
            conn.Close();

            // if count 1, the game exists. if count is 0 , it does not exist.
            return count > 0;
        }
        public void CreateUserGame(UserGameDto dto)
        {
            const string sql = @"INSERT INTO UserGame (UserId, GameId, Status, Platform, PricePaid, PurchacedAt, AddedAt, HoursPlayed, Notes)
                                 VALUES (@UserId, @GameId, @Status, @Platform, @PricePaid, @PurchacedAt, @AddedAt, @HoursPlayed, @Notes)";
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", dto.UserId);
            cmd.Parameters.AddWithValue("@GameId", dto.GameId);
            cmd.Parameters.AddWithValue("@Status", (int)dto.Status);
            cmd.Parameters.AddWithValue("@Platform", (int)dto.Platform);
            cmd.Parameters.AddWithValue("@PricePaid", dto.PricePaid);
            cmd.Parameters.AddWithValue("@PurchacedAt", dto.PurchacedAt);
            cmd.Parameters.AddWithValue("@AddedAt", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@HoursPlayed", dto.HoursPlayed);
            cmd.Parameters.AddWithValue("@Notes", dto.Notes ?? (object)DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void UpdateUserGame(UserGameDto dto)
        {
            const string sql = @"UPDATE UserGame 
                                 SET Status = @Status, 
                                     Platform = @Platform, 
                                     PricePaid = @PricePaid, 
                                     PurchacedAt = @PurchacedAt, 
                                     HoursPlayed = @HoursPlayed, 
                                     Notes = @Notes
                                 WHERE UserId = @UserId AND GameId = @GameId";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@UserId", dto.UserId);
            cmd.Parameters.AddWithValue("@GameId", dto.GameId);
            cmd.Parameters.AddWithValue("@Status", dto.Status);
            cmd.Parameters.AddWithValue("@Platform", dto.Platform);
            cmd.Parameters.AddWithValue("@PricePaid", dto.PricePaid);
            cmd.Parameters.AddWithValue("@PurchacedAt", dto.PurchacedAt);
            cmd.Parameters.AddWithValue("@HoursPlayed", dto.HoursPlayed);
            cmd.Parameters.AddWithValue("@Notes", (object?)dto.Notes ?? DBNull.Value);


            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void DeleteUserGame(int userId, int gameId)
        {
            const string sql = "DELETE FROM UserGame WHERE UserId = @UserId AND GameId = @GameId";
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@GameId", gameId);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
