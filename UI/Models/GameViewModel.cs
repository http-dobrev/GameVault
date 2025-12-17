using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Models
{
    public class GameViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int GenreId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int DeveloperId { get; set; }
        public int PublisherId { get; set; }
        public decimal Price { get; set; }
        public int PegiAge { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsArchived { get; set; }

        // For dropdown options
        public List<SelectListItem> Developers { get; set; } = [];
        public List<SelectListItem> Genres { get; set; } = [];
        public List<SelectListItem> Publishers { get; set; } = [];
        public IEnumerable<SelectListItem>? PegiOptions { get; set; }
    }
}
