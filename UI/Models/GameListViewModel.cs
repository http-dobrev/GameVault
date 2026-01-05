namespace UI.Models
{
    public class GameListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string GenreName { get; set; } = string.Empty;

        public DateTime ReleaseDate { get; set; }

        public string DeveloperName { get; set; } = string.Empty;
        public string PublisherName { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public int PegiAge { get; set; }
        public string Description {  get; set; } = string.Empty;
        public string CoverImageURL {  get; set; } = string.Empty;
    }
}
