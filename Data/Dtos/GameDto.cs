
namespace Data.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int GenreId { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public int DeveloperId { get; set; }
        public int PublisherId { get; set; }
        public decimal Price { get; set; }
        public int PegiAge { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int IsArchived { get; set; }
    }
}
