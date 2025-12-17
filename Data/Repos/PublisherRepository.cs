using Data.Dtos;
using Data.Mappers;
using Logic.Entities;
using Logic.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Data.Repos
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly string _connectionString;

        public PublisherRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }

        public IEnumerable<Publisher> GetAllPublishers()
        {
            var publishers = new List<Publisher>();

            const string sql = "SELECT * FROM Publisher";

            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);

            conn.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var dto = new PublisherDto
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                };
                publishers.Add(PublisherDataMapper.ToEntity(dto));
            }

            reader.Close();
            conn.Close();

            return publishers;
        }
    }
}
