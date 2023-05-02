namespace RottenTomatoes.Models
{
    public class Movie: Show
    {
        public Movie(string Title, string ImageUrl, string TomatometerScore, string 
            AudienceScore, List<string> Platforms, string Synopsis, string Clasification, 
            string Genre, string OriginalLanguage, string Director, string ReleaseDate, 
            string Runtime, Dictionary<string, string> actorRoles, List<string> criticReview, List<string> audienceReview) { 

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
            this.actorRoles = actorRoles;
            this.criticReview = criticReview;
            this.audienceReview = audienceReview;           
        }
    }
}
