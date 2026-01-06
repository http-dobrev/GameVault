using Logic.Entities;

namespace UI.Models
{
    public class LibraryListItemVM
    {
        public int GameId { get; set; }
        public string CoverImageURL { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string GenreName { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal PricePaid { get; set; }
        public int HoursPlayed { get; set; }
        public DateTime OwnedSince { get; set; }
        public DateTime AddedAt { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
