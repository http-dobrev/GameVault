using Logic.Entities;

namespace UI.Models
{
    public class LibraryListItemVM
    {
        public int GameId;
        public string Title = string.Empty;
        public string GenreName = string.Empty;
        public string Status = string.Empty;
        public string Platform = string.Empty;
        public decimal PricePaid;
        public DateTime OwnedSince;
        public DateTime AddedAt;
        public int HoursPlayed;
        public string Notes = string.Empty;
        public string CoverImageURL = string.Empty;
    }
}
