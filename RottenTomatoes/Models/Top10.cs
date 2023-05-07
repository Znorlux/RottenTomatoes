namespace RottenTomatoes.Models
{
    public class Top10
    {
        public int Top10Id { get; set; }
        public string title { get; set; }
        public string url { get; set; }

        public Top10()
        {}

        public Top10(string title, string url)
        {
            this.title = title;
            this.url = url;
        }
    }
}
