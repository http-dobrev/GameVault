using Data.Dtos;
using Data.Mappers;
using Logic.Entities;
using Logic.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Data.Repos
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }

        public bool ExistsByEmail(string email)
        {
            const string sql = "SELECT COUNT(1) FROM [User] WHERE Email = @Email";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Email", email);

            conn.Open();
            int count = (int)cmd.ExecuteScalar();

            return count > 0;
        }

        public bool ExistsByUsername(string username)
        {
            const string sql = "SELECT COUNT(1) FROM [User] WHERE Username = @Username";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Username", username);

            conn.Open();
            int count = (int)cmd.ExecuteScalar();

            return count > 0;
        }

        public User? GetByEmail(string email)
        {
            const string sql = @"
                SELECT Id, Username, Email, PasswordHash, Role, CreatedAt, IsArchived
                FROM [User]
                WHERE Email = @Email";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Email", email);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            var dto = new UserDto
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                Role = reader.GetInt32(reader.GetOrdinal("Role")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                IsArchived = reader.GetBoolean(reader.GetOrdinal("IsArchived"))
            };

            return UserDataMapper.UserDtoToEntity(dto);
        }

        public User? GetByUsername(string username)
        {
            const string sql = @"
                SELECT Id, Username, Email, PasswordHash, Role, CreatedAt, IsArchived
                FROM [User]
                WHERE Username = @Username";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Username", username);
            
            conn.Open();
            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            var dto = new UserDto
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                Role = reader.GetInt32(reader.GetOrdinal("Role")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                IsArchived = reader.GetBoolean(reader.GetOrdinal("IsArchived"))
            };

            return UserDataMapper.UserDtoToEntity(dto);
        }

        public int CreateUser(User user)
        {
            var dto = UserDataMapper.UserEntityToDto(user);

            const string sql = @"
                INSERT INTO [User] (Username, Email, PasswordHash, Role, CreatedAt, IsArchived)
                OUTPUT INSERTED.Id
                VALUES (@Username, @Email, @PasswordHash, @Role, @CreatedAt, @IsArchived)";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Username", dto.Username);
            cmd.Parameters.AddWithValue("@Email", dto.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", dto.PasswordHash);
            cmd.Parameters.AddWithValue("@Role", dto.Role);
            cmd.Parameters.AddWithValue("@CreatedAt", dto.CreatedAt);
            cmd.Parameters.AddWithValue("@IsArchived", dto.IsArchived);

            conn.Open();

            // return new id
            return (int)cmd.ExecuteScalar();
        }
    }
}
