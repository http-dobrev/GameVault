using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Models
{
    public class UserGameEditViewModel
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public int? Status { get; set; }
        public int? Platform { get; set; }
        public decimal? PricePaid { get; set; }
        public DateTime? PurchacedAt { get; set; }
        public DateTime AddedAt { get; set; }
        public int? HoursPlayed { get; set; }
        public string? Notes { get; set; } = string.Empty;

        // for dropdown options
        public IEnumerable<SelectListItem>? StatusOptions { get; set; }
        public IEnumerable<SelectListItem>? PlatformOptions { get; set; }
    }
}
