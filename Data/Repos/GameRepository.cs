using Logic.Dtos;
using Logic.Mappers;
using Logic.Entities;
using Logic.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace Data.Repos
{
    public class GameRepository : IGameRepository
    {
        private readonly string _connectionString;

        public GameRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }

        public IEnumerable<GameDto> GetAllGames()
        {
            var games = new List<GameDto>();
            const string sql = @"SELECT
                                    g.Id,
                                    g.Title,
                                    g.GenreId,
                                    ge.Name AS GenreName,
                                    g.DeveloperId,
                                    d.Name AS DeveloperName,
                                    g.PublisherId,
                                    p.Name AS PublisherName,
                                    g.ReleaseDate,
                                    g.Price,
                                    g.PegiAge,
                                    g.Description,
                                    g.CoverImageUrl,
                                    g.CreatedAt,
                                    g.UpdatedAt,
                                    g.IsArchived
                                FROM Game g
                                INNER JOIN Genre ge ON ge.Id = g.GenreId
                                INNER JOIN Developer d  ON d.Id  = g.DeveloperId
                                INNER JOIN Publisher p  ON p.Id  = g.PublisherId
                                WHERE g.IsArchived = 0
                                ORDER BY g.Title;";
            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var dto = new GameDto
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    GenreName = (string)reader["GenreName"],
                    ReleaseDate = (DateTime)reader["ReleaseDate"],
                    DeveloperName = (string)reader["DeveloperName"],
                    PublisherName = (string)reader["PublisherName"],
                    Price = (decimal)reader["Price"],
                    PegiAge = (byte)reader["PegiAge"],
                    Description = (string)reader["Description"],
                    CoverImageUrl = (string)reader["CoverImageUrl"],
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    UpdatedAt = (DateTime)reader["UpdatedAt"],
                    IsArchived = (bool)reader["IsArchived"]
                };
                games.Add(dto);
            }
            reader.Close();
            conn.Close();
            return games;
        }

        public void CreateGame(GameDto dto)
        {
            const string sql = @"
                INSERT INTO Game (Title, GenreId, ReleaseDate, DeveloperId, PublisherId, Price, PegiAge, Description, CoverImageUrl, CreatedAt, UpdatedAt, IsArchived)
                VALUES (@Title, @GenreId, @ReleaseDate, @DeveloperId, @PublisherId, @Price, @PegiAge, @Description, @CoverImageUrl, @CreatedAt, @UpdatedAt, @IsArchived)";

            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Title", dto.Title);
            cmd.Parameters.AddWithValue("@GenreId", dto.GenreId);
            cmd.Parameters.AddWithValue("@ReleaseDate", dto.ReleaseDate);
            cmd.Parameters.AddWithValue("@DeveloperId", dto.DeveloperId);
            cmd.Parameters.AddWithValue("@PublisherId", dto.PublisherId);
            cmd.Parameters.AddWithValue("@Price", dto.Price);
            cmd.Parameters.AddWithValue("@PegiAge", dto.PegiAge);
            cmd.Parameters.AddWithValue("@Description", dto.Description);
            cmd.Parameters.AddWithValue("@CoverImageUrl", dto.CoverImageUrl);
            cmd.Parameters.AddWithValue("@CreatedAt", dto.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", dto.UpdatedAt);
            cmd.Parameters.AddWithValue("@IsArchived", dto.IsArchived);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public GameDto? GetGame(int id)
        {
            const string sql = @"SELECT
                                    g.Id,
                                    g.Title,
                                    g.GenreId,
                                    ge.Name AS GenreName,
                                    g.DeveloperId,
                                    d.Name AS DeveloperName,
                                    g.PublisherId,
                                    p.Name AS PublisherName,
                                    g.ReleaseDate,
                                    g.Price,
                                    g.PegiAge,
                                    g.Description,
                                    g.CoverImageUrl,
                                    g.CreatedAt,
                                    g.UpdatedAt,
                                    g.IsArchived
                                FROM Game g
                                INNER JOIN Genre ge ON ge.Id = g.GenreId
                                INNER JOIN Developer d  ON d.Id  = g.DeveloperId
                                INNER JOIN Publisher p  ON p.Id  = g.PublisherId
                                WHERE g.Id = @Id AND g.IsArchived = 0";

            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            var reader = cmd.ExecuteReader();

            if (!reader.Read()) return null;

            var dto = new GameDto
            {
                Id = (int)reader["Id"],
                Title = (string)reader["Title"],
                GenreId = (int)reader["GenreId"],
                GenreName = (string)reader["GenreName"],
                ReleaseDate = (DateTime)reader["ReleaseDate"],
                DeveloperId = (int)reader["DeveloperId"],
                DeveloperName = (string)reader["DeveloperName"],
                PublisherId = (int)reader["PublisherId"],
                PublisherName = (string)reader["PublisherName"],
                Price = (decimal)reader["Price"],
                PegiAge = (byte)reader["PegiAge"],
                Description = (string)reader["Description"],
                CoverImageUrl = (string)reader["CoverImageUrl"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                UpdatedAt = (DateTime)reader["UpdatedAt"],
                IsArchived = (bool)reader["IsArchived"]
            };

            reader.Close();
            conn.Close();

            return dto;
        }

        public bool GameExists(string title)
        {
            const string sql = "SELECT COUNT(1) FROM Game WHERE Title = @Title AND IsArchived = 0";

            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Title", title);

            conn.Open();
            int count = (int)cmd.ExecuteScalar();
            conn.Close();

            // if count > 0, the game exists. if count is 0 , it does not exist.
            return count > 0;
        }

        public void UpdateGame(GameDto dto)
        {

            const string sql = @"UPDATE Game
                                 SET Title = @Title,GenreId = @GenreId,ReleaseDate = @ReleaseDate,DeveloperId = @DeveloperId,PublisherId = @PublisherId,Price = @Price,PegiAge = @PegiAge,Description = @Description,CoverImageUrl = @CoverImageUrl,UpdatedAt = @UpdatedAt,IsArchived = @IsArchived
                                 WHERE Id = @Id";

            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Id", dto.Id);
            cmd.Parameters.AddWithValue("@Title", dto.Title);
            cmd.Parameters.AddWithValue("@GenreId", dto.GenreId);
            cmd.Parameters.AddWithValue("@ReleaseDate", dto.ReleaseDate);
            cmd.Parameters.AddWithValue("@DeveloperId", dto.DeveloperId);
            cmd.Parameters.AddWithValue("@PublisherId", dto.PublisherId);
            cmd.Parameters.AddWithValue("@Price", dto.Price);
            cmd.Parameters.AddWithValue("@PegiAge", dto.PegiAge);
            cmd.Parameters.AddWithValue("@Description", dto.Description);
            cmd.Parameters.AddWithValue("@CoverImageUrl", dto.CoverImageUrl);
            cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
            cmd.Parameters.AddWithValue("@IsArchived", dto.IsArchived);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void ArchiveGame(int id)
        {
            const string sql = @"UPDATE Game SET IsArchived = 1 WHERE Id = @Id";

            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
