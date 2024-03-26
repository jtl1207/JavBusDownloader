using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data
{
    #region get /api/movies => ApiMovies
    public partial class ApiMovies
    {
        public string Keyword { get; set; }
        public Movie[] Movies { get; set; }
        public Pagination Pagination { get; set; }
    }

    public partial class Movie
    {
        public string Date { get; set; }
        public string Id { get; set; }
        public string Img { get; set; }
        public string[] Tags { get; set; }
        public string Title { get; set; }
    }

    public partial class Pagination
    {
        public long? CurrentPage { get; set; }
        public bool? HasNextPage { get; set; }
        public long? NextPage { get; set; }
        public long[] Pages { get; set; }
    }
    #endregion

    #region get /api/movies/AAA-001 => ApiMovieInfo
    public partial class ApiMovieInfo
    {
        public string Date { get; set; }
        public Director Director { get; set; }
        public Genre[] Genres { get; set; }
        public string Gid { get; set; }
        public string Id { get; set; }
        public ImageSize ImageSize { get; set; }
        public string Img { get; set; }
        public Producer Producer { get; set; }
        public Publisher Publisher { get; set; }
        public Sample[] Samples { get; set; }
        public Series Series { get; set; }
        public Star[] Stars { get; set; }
        public string Title { get; set; }
        public string Uc { get; set; }
        public long? VideoLength { get; set; }
    }

    public partial class Director
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public partial class Genre
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public partial class ImageSize
    {
        public long Height { get; set; }
        public long Width { get; set; }
    }

    public partial class Producer
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public partial class Publisher
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public partial class Sample
    {
        public string Alt { get; set; }
        public string Id { get; set; }
        public string Src { get; set; }
        public string Thumbnail { get; set; }
    }

    public partial class Series
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public partial class Star
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    #endregion

    #region get /api/magnets/{movieId} => ApiMagnets
    public partial class ApiMagnets
    {
        public bool? HasSubtitle { get; set; }
        public string Id { get; set; }
        public bool? IsHd { get; set; }
        public string Link { get; set; }
        public long? NumberSize { get; set; }
        public string ShareDate { get; set; }
        public string Size { get; set; }
        public string Title { get; set; }
    }
    #endregion

    public partial class ApiMovies
    {
        public static ApiMovies FromJson(string json) => JsonConvert.DeserializeObject<ApiMovies>(json, Data.Converter.Settings);
    }
    public partial class ApiMovieInfo
    {
        public static ApiMovieInfo FromJson(string json) => JsonConvert.DeserializeObject<ApiMovieInfo>(json, Data.Converter.Settings);
    }
    public partial class ApiMagnets
    {
        public static ApiMagnets[] FromJson(string json) => JsonConvert.DeserializeObject<ApiMagnets[]>(json, Data.Converter.Settings);
    }



    public static class Serialize
    {
        public static string ToJson(this ApiMovies self) => JsonConvert.SerializeObject(self, Data.Converter.Settings);
        public static string ToJson(this ApiMovieInfo self) => JsonConvert.SerializeObject(self, Data.Converter.Settings);
        public static string ToJson(this ApiMagnets[] self) => JsonConvert.SerializeObject(self, Data.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
