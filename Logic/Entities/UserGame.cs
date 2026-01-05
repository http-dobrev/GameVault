using Logic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Entities
{
    public class UserGame
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public UserGameStatus Status { get; set; }
        public UserGamePlatform Platform { get; set; }
        public decimal PricePaid { get; set; }
        public DateTime PurchacedAt { get; set; }
        public DateTime AddedAt { get; set; }
        public int HoursPlayed { get; set; }
        public string Notes { get; set; } = string.Empty;
        public Game? Game { get; set; } = new();
    }
}
