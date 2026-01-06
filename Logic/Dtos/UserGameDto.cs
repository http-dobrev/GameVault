
namespace Logic.Dtos
{
    public class UserGameDto
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public int Status { get; set; }
        public int Platform { get; set; } 
        public decimal PricePaid { get; set; }
        public DateTime PurchacedAt { get; set; }
        public DateTime AddedAt { get; set; }
        public int HoursPlayed { get; set; }
        public string Notes { get; set; } = string.Empty;
        public GameDto Game { get; set; } = new();
    }
}
