
namespace Logic.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public int GenreId { get; set; }
        public string GenreName { get; set; } = string.Empty;

        public DateTime ReleaseDate { get; set; }

        public int DeveloperId { get; set; }
        public string DeveloperName { get; set; } = string.Empty;

        public int PublisherId { get; set; }
        public string PublisherName { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public int PegiAge { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsArchived { get; set; }
    }
}
