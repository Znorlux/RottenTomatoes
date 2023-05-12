using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using RottenTomatoes.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;

class WebScraper
{
    private string _showType;
    private string _url;

    public WebScraper(string showType, string url)
    {
        _showType = showType;
        _url = url;
    }

    public async Task<Show> GetShowInfo()
    {
        switch (_showType)
        {
            case "pelicula":
                Movie movie = await getMovieInfo(_url);
                return movie;
            case "serie":
                Serie serie = await getSeriesInfo(_url);
                return serie;
            //case "top10":
                //List <List<Top10>> = getTop10();
                //return top10;
            default:
                // mostrar un mensaje de error o lanzar una excepción
                return null;
        }
    }
    
    static async Task Main(string[] args)
    {
        //await getMovieInfo();
        //await getSeriesInfo();
        //await getTop10();
    }
    public static async Task<Movie> getMovieInfo(string link)
    {
        var url = link;
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(url);
        //En html está guardado el codigo completo tras la petición

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var MovieTitleElement = htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='title']").InnerText;
        var MovieTitle = WebUtility.HtmlDecode(MovieTitleElement); //utilizamos WebUtility.HtmlDecode() porque hay peliculas que
                                                                   //tienen carecteres especiales en sus titulos, asi los devolvemos a como son

       
        var ImageUrl = htmlDocument.DocumentNode.SelectSingleNode("//img[@alt='Watch trailer for " + MovieTitleElement + "' and @slot='image']")
                            .GetAttributeValue("src", "");
        

        var scoreBoardElement = htmlDocument.DocumentNode.SelectSingleNode("//score-board");

        var tomatometerScore = scoreBoardElement.GetAttributeValue("tomatometerscore", "");

        var AudienceScore = scoreBoardElement.GetAttributeValue("audiencescore", "");

        var whereToWatchSection = htmlDocument.DocumentNode.SelectSingleNode("//section[@id='where-to-watch']");


        List<String> platforms = new List<String>();
        var platformElements = whereToWatchSection.SelectNodes(".//where-to-watch-meta");
        foreach (var platformElement in platformElements)
        {
            var platformUrl = platformElement.GetAttributeValue("href", "");
            //En el atributo image se encuentra el nombre de la plataforma donde se puede ver el Show
            var platform = platformElement.SelectSingleNode(".//where-to-watch-bubble").GetAttributeValue("image", "");
            platforms.Add(platform);
        }
        //Se desciende sobre el nodo raiz hasta encontrar un nodo el cual tenga un atributo llamado "data-qa" cuyo valor sea "movie-info-synopsis" que es donde se encuentra la sinopsis
        var synopsisNode = htmlDocument.DocumentNode.Descendants()//Puede servir para bajar por todo el DOM y buscar los que cumplan con ciertas condiciones
            .FirstOrDefault(n => n.GetAttributeValue("data-qa", "") == "movie-info-synopsis");

        var synopsis = synopsisNode?.InnerText?.Trim();//El Trim solo es borrar los espacios vacios

        //Mucha de la información de la pelicula se encuentra en el elemento ul (lista desordenada de HTML)
        //con id=info, por lo tanto accederemos a cada elemento que necesitemos

        var infoList = htmlDocument.DocumentNode.SelectSingleNode("//ul[@id='info']");//Nodo de la lista con la info

        var ratingLabel = infoList.SelectSingleNode(".//b[contains(@data-qa, 'movie-info-item-label') and text()='Rating:']");
        var ratingValue = ratingLabel?.ParentNode?.SelectSingleNode(".//span[contains(@data-qa, 'movie-info-item-value')]");
        var rating = ratingValue?.InnerText.Trim();
        if(rating == null)
        {
            rating = "Undefined";
        }

        var genreLabel = infoList.SelectSingleNode(".//b[contains(@data-qa, 'movie-info-item-label') and text()='Genre:']");
        var genreValue = genreLabel?.ParentNode?.SelectSingleNode(".//span[contains(@data-qa, 'movie-info-item-value')]");
        var genre = genreValue?.InnerText.Trim();
        //genre = genre?.Replace(",", ", ");
        genre = Regex.Replace(genre, @"\s+", " ");
        genre = genre?.Replace(",", ", ");
        genre = WebUtility.HtmlDecode(genre);

        var languageLabel = infoList.SelectSingleNode(".//b[contains(@data-qa, 'movie-info-item-label') and text()='Original Language:']");
        var languageValue = languageLabel?.ParentNode?.SelectSingleNode(".//span[contains(@data-qa, 'movie-info-item-value')]");
        var language = languageValue?.InnerText.Trim();

        var directorLabel = infoList.SelectSingleNode(".//b[contains(@data-qa, 'movie-info-item-label') and text()='Director:']");
        var directorValue = directorLabel?.ParentNode?.SelectSingleNode(".//a[contains(@data-qa, 'movie-info-director')]");
        var director = directorValue?.InnerText.Trim();

        //Caso en que haya salido en cines
        var releaseLabel = infoList.SelectSingleNode(".//b[contains(@data-qa, 'movie-info-item-label') and text()='Release Date (Theaters):']");
        var releaseValue = releaseLabel?.ParentNode?.SelectSingleNode(".//time");
        var releaseDate = releaseValue?.Attributes["datetime"]?.Value.Trim();
        if (releaseDate == null)
        {
            releaseDate = "Fecha indefinida";
        }

        var runtimeLabel = infoList.SelectSingleNode(".//b[contains(@data-qa, 'movie-info-item-label') and text()='Runtime:']");
        var runtimeValue = runtimeLabel?.ParentNode?.SelectSingleNode(".//time");
        var runtime = runtimeValue?.InnerText.Trim();

        //Pasamos a la parte final de la pagina

        Dictionary<String, String> actorRolList = new Dictionary<String, String>();
        var castSection = htmlDocument.DocumentNode.SelectSingleNode("//div[@data-qa='cast-section']");
        if (castSection != null)
        {
            //Console.WriteLine("Actores principales:");
            var castCrewItems = castSection.SelectNodes(".//div[@class='cast-and-crew-item ']");
            if (castCrewItems != null)
            {
                //moreCasts hide
                foreach (var item in castCrewItems)
                {
                    String actorName = "";
                    String actorRol = "";
                    var actorImg = item.SelectSingleNode(".//img");
                    if (actorImg != null)
                    {
                        actorName = actorImg.GetAttributeValue("alt", "");//El valor del atributo alt contiene el nombre del actor

                    }
                    var actorRolNode = item.SelectSingleNode(".//p[@class='p--small']");
                    if (actorRolNode != null)
                    {
                        var actorRolEspaciado = actorRolNode.InnerText.Trim();
                        //regex para eliminar espacios innecesarios entre palabras y dejar solo uno
                        actorRol = Regex.Replace(actorRolEspaciado, @"\s+", " ");

                    }
                    actorRolList.Add(actorName, actorRol);
                }
            }
        }
        //Console.WriteLine("");
        var criticReview = htmlDocument.DocumentNode.SelectNodes("//review-speech-balloon[@data-qa='critic-review' and @istopcritic= 'true']");

        List<String> criticReviews = new List<String>();

        List<String> audienceReviews = new List<String>();
        if (criticReview != null)
        {
            for (int i = 0; i < 3 && i < criticReview.Count; i++)
            {
                var reviewQuote = criticReview[i]?.GetAttributeValue("reviewquote", "");
                reviewQuote = WebUtility.HtmlDecode(reviewQuote);
                reviewQuote = reviewQuote?.Trim();
                criticReviews.Add(reviewQuote);
            }
            var audienceReview = htmlDocument.DocumentNode.SelectNodes("//review-speech-balloon[@data-qa='critic-review' and @istopcritic= 'false']");
            if (audienceReview != null)
            {
                for (int i = 0; i < 3 && i < audienceReview.Count; i++)
                {
                    var audienceQuote = audienceReview[i]?.GetAttributeValue("reviewquote", "");
                    audienceQuote = WebUtility.HtmlDecode(audienceQuote);
                    audienceReviews.Add(audienceQuote);
                }
            }
        }

        string platforms_string = string.Join(", ", platforms);
        string criticReview_string = string.Join("\n- ", criticReviews);
        string audienceReview_string = string.Join("\n- ", audienceReviews);
        string actorRol_string = "";
        foreach (KeyValuePair<string, string> par in actorRolList)
        {
            actorRol_string += par.Key + " - " + par.Value + "\n";
        }
        actorRol_string = actorRol_string.TrimEnd();

        Movie movie = new Movie(MovieTitle, ImageUrl, tomatometerScore, AudienceScore, platforms_string, synopsis, rating,
                genre, language, director, releaseDate, runtime, actorRol_string, criticReview_string, audienceReview_string);
        return movie;
    }

    static async Task<Serie> getSeriesInfo(string link)
    {
        var url = link;
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(url);


        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var SerieTitleElement = htmlDocument.DocumentNode.SelectSingleNode("//h1[@slot='title']").InnerText;
        var SerieTitle = WebUtility.HtmlDecode(SerieTitleElement);

        string ImageUrl = null;

        try
        {
            ImageUrl = htmlDocument.DocumentNode.SelectSingleNode("//img[@data-qa='poster-image' and @slot='image']")
                                .GetAttributeValue("src", "");
            //ImageUrl = htmlDocument.DocumentNode.SelectSingleNode("//img[@data-qa='Watch trailer for " + SerieTitle + "' and @slot='image']")
                                //.GetAttributeValue("src", "");
        }
        catch (NullReferenceException)
        {
            // La selección del nodo falló, se asigna un valor nulo a ImageUrl
        }

        if (string.IsNullOrEmpty(ImageUrl))
        {
            // Casos especiales donde el título de la serie tenga caracteres especiales como "&"
            ImageUrl = htmlDocument.DocumentNode.SelectSingleNode("//img[@alt='Watch trailer for " + SerieTitle.Replace("&", "&amp;") + "' and @slot='image']")
                                ?.GetAttributeValue("src", "");
        }

        var scoreBoardElement = htmlDocument.DocumentNode.SelectSingleNode("//score-board");

        var tomatometerScore = scoreBoardElement.GetAttributeValue("tomatometerscore", "");

        var AudienceScore = scoreBoardElement.GetAttributeValue("audiencescore", "");

        var whereToWatchSection = htmlDocument.DocumentNode.SelectSingleNode("//section[@id='where-to-watch']");

        var platformElements = whereToWatchSection?.SelectNodes(".//where-to-watch-meta");
        

        List<String> platforms = new List<String>();
        if (platformElements != null)
        {
            foreach (var platformElement in platformElements)
            {
                var platformUrl = platformElement.GetAttributeValue("href", "");
                //En el atributo image se encuentra el nombre de la plataforma donde se puede ver el Show
                var platform = platformElement.SelectSingleNode(".//where-to-watch-bubble").GetAttributeValue("image", "");
                platforms.Add(platform);
            }
        }
        

        var synopsisNode = htmlDocument.DocumentNode.Descendants()
            .FirstOrDefault(n => n.GetAttributeValue("data-qa", "") == "series-info-description");

        var synopsis = synopsisNode?.InnerText?.Trim();

        var creatorValue = htmlDocument.DocumentNode.SelectSingleNode("//li[b[@data-qa='series-info-creators']]//span[@class='info-item-value']/a/span");
        var creator = creatorValue?.InnerText.Trim();
        if(creator == null)
        {
            creator = "No disponible";
        }
        var starringNode = htmlDocument.DocumentNode.SelectSingleNode("//li[contains(., 'Starring: ')]");
        var starringLinks = starringNode?.SelectNodes(".//a");


        // Iterar sobre los elementos <a> y extraer el texto de los elementos <span>
        List<string> actors = new List<string>();
        if (starringLinks != null)
        {
            foreach (HtmlNode starring in starringLinks)
            {
                var span = starring.SelectSingleNode(".//span");
                string actorName = span.InnerText.Trim();
                actors.Add(actorName);
            }
        }
        
        var tvNetworkValue = htmlDocument.DocumentNode.SelectSingleNode("//li[contains(., 'TV Network: ')]");
        var tvNetwork = tvNetworkValue?.InnerText.Replace("TV Network: ", "").Trim(); //con el replace hacemos que no se guarde el "TV network"
                                                                                      //y borramos espacios en blanco

        var premiereDateValue = htmlDocument.DocumentNode.SelectSingleNode("//li[contains(., 'Premiere Date: ')]");
        var premiereDate = premiereDateValue?.InnerText.Replace("Premiere Date: ", "").Trim();

        var genreValue = htmlDocument.DocumentNode.SelectSingleNode("//li[contains(., 'Genre: ')]");
        var genre = genreValue?.InnerText.Replace("Genre: ", "").Trim();

        string platforms_string = "";
        if(platforms.Count !=0)
        {
            platforms_string = string.Join(", ", platforms);
        }
        else
        {
            platforms_string = "No disponible";
        }
        string actors_string = "";
        if (actors.Count != 0)
        {
            actors_string = string.Join("\n", actors);
        }
        else
        {
            actors_string = "No disponible";
        }
        
        Serie serie = new Serie(SerieTitle, ImageUrl, tomatometerScore, AudienceScore, platforms_string, synopsis, genre,
            creator, actors_string, premiereDate);
        return serie;
    }
    public async Task<List<List<Top10>>> getTop10()
    {
        var url = "https://www.rottentomatoes.com";
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(url);

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        List<Top10> top10moviesList = new List<Top10>();
        for (int i = 1; i <= 10; i++)
        {
            //Iteramos sobre cada nodo que cumple con la condicion de ese value de class
            //Solamente iteraremos 10 veces debido a que en estos 10 primeros nodos está el top de peliculas
            var topValue = htmlDocument.DocumentNode.SelectSingleNode($"(//span[@class='dynamic-text-list__item-title clamp clamp-1'])[{i}]");
            var topMovie = topValue?.InnerText.Trim();
            topMovie = WebUtility.HtmlDecode(topMovie);
            //Console.WriteLine("Titulo: "+topMovie);
            var linkNode = htmlDocument.DocumentNode.SelectSingleNode($"(//a[@class='dynamic-text-list__tomatometer-group'])[{i}]");
            var movieLink = linkNode?.GetAttributeValue("href", "");//aqui solamente se almacena "/m/{nombre_pelicula}", asi que falta completar el link
            var fullLink = "https://www.rottentomatoes.com" + movieLink;
            Top10 top10movie = new Top10(topMovie, fullLink);//Titulo + link
            top10moviesList.Add(top10movie);
        }
        List<Top10> top10seriesList = new List<Top10>();
        for (int i = 11; i <= 20; i++)
        {

            var serieValue = htmlDocument.DocumentNode.SelectSingleNode($"(//span[@class='dynamic-text-list__item-title clamp clamp-1'])[{i}]");
            var topSerie = serieValue?.InnerText.Trim();
            topSerie = WebUtility.HtmlDecode(topSerie);
            var linkNode = htmlDocument.DocumentNode.SelectSingleNode($"(//a[@class='dynamic-text-list__tomatometer-group'])[{i}]");
            var serieLink = linkNode?.GetAttributeValue("href", "");
            var fullLink = "https://www.rottentomatoes.com" + serieLink;
            //en full link puede quedar por ejemplo algo como https://www.rottentomatoes.com/tv/silo/s01
            fullLink = fullLink.Substring(0, fullLink.LastIndexOf("/"));
            Top10 top10serie = new Top10(topSerie, fullLink);
            top10seriesList.Add(top10serie);
        }
        List<List<Top10>> top10list = new List<List<Top10>>();//Lista que almacenara dos listas de top, una de movies y otra de las series
        top10list.Add(top10moviesList);
        top10list.Add(top10seriesList);
        return top10list;
    }

}