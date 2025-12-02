using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Entities
{
    public class Game
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
