using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace RottenTomatoes.Models
{
    public class Movie : Show
    {

        public int MovieId { get; set; }
        public string CriticReview { get; set; }
        public string AudienceReview { get; set; }
        public string Clasification { get; set; }
        public string OriginalLanguage { get; set; }
        public string Director { get; set; }
        public string Runtime { get; set; }
        //actor - rol      
        public string ActorRoles { get; set; }
        public virtual ICollection<FavoriteMovie> FavoriteMovie { get; set; }

        public Movie() {
            this.FavoriteMovie = new HashSet<FavoriteMovie>();
        }

        public Movie(string Title, string ImageUrl, string TomatometerScore, string
            AudienceScore, string Platforms, string Synopsis, string Clasification,
            string Genre, string OriginalLanguage, string Director, string ReleaseDate,
            string Runtime, string ActorRoles, string CriticReview, string AudienceReview)
        {
            this.Title = Title;
            this.ImageURL = ImageUrl;
            this.TomatometerScore = TomatometerScore;
            this.AudienceScore = AudienceScore;
            this.Platforms = Platforms;
            this.Synopsis = Synopsis;
            this.Clasification = Clasification;
            this.Genre = Genre;
            this.OriginalLanguage = OriginalLanguage;
            this.Director = Director;
            this.ReleaseDate = ReleaseDate;
            this.Runtime = Runtime;
            this.ActorRoles = ActorRoles;
            this.CriticReview = CriticReview;
            this.AudienceReview = AudienceReview;
            this.FavoriteMovie = new HashSet<FavoriteMovie>();

        }
    }
}





