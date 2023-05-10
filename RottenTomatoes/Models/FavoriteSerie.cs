using System.ComponentModel.DataAnnotations;

namespace RottenTomatoes.Models
{
    public class FavoriteSerie
    {
        public int Id { get; set; }

        // fk user
        public int UserId { get; set; }
        public virtual User? User { get; set; }

        // fk movie
        public int SerieId { get; set; }
        public virtual Serie? Serie { get; set; }

        public FavoriteSerie(int UserId, int SerieId)
        {
            this.UserId = UserId;
            this.SerieId = SerieId;
        }
    }
}
