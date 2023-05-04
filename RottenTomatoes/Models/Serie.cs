using System.ComponentModel.DataAnnotations.Schema;

namespace RottenTomatoes.Models
{
    public class Serie : Show
    {
        public int SerieId { get; set; }
        public string Creator { get; set; }
        public string mainActors { get; set; }
        public Serie() { }

        public Serie(string Title, string ImageUrl, string TomatometerScore, string
            AudienceScore, string Platforms, string Synopsis,
            string Genre, string Creator, string mainActors, string ReleaseDate)
        {

            this.Title = Title;
            this.ImageURL = ImageUrl;
            this.TomatometerScore = TomatometerScore;
            this.AudienceScore = AudienceScore;
            this.Platforms = Platforms;
            this.Synopsis = Synopsis;
            this.Genre = Genre;
            this.Creator = Creator;
            this.mainActors = mainActors;
            this.ReleaseDate = ReleaseDate;
            
        }
        
    }
}
