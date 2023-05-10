using System.ComponentModel.DataAnnotations;

namespace RottenTomatoes.Models
{
    public class FavoriteMovie
    {
        public int Id { get; set; }

        // fk user
        public int UserId { get; set; }
        public virtual User? User { get; set; }

        // fk movie
        public int MovieId { get; set; }
        public virtual Movie? Movie { get; set; }

        public FavoriteMovie(int UserId, int MovieId) {
            this.UserId = UserId;
            this.MovieId = MovieId;
        }
    }
}
