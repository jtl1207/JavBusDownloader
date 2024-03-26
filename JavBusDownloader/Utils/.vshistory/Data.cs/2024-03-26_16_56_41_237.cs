// var movieData = MovieData.FromJson(jsonString);
// var magnet = Magnet.FromJson(jsonString);
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

namespace Data
{
    class Movies
    {
        int state = 0; // -1：失败 0：加载数据 1：加载种子 1：重试中 2：加载成功
        string date = "2024-04-27";
        string id = "BDST-012";
        string img = "https://www.javbus.com/pics/thumb/aerq.jpg";
        string[] tags = { };
        string title = "完全引退 AV女優、最後の1日。三上悠亜ラストセックス";
        MovieData movieData = null;
        Magnet[] magnets = { };
    }

    #region /api/movies
    public partial class ApiMovies
    {
        [J("movies")] public Movie[] Movies { get; set; }
        [J("pagination")] public Pagination Pagination { get; set; }
    }

    public partial class Movie
    {
        [J("date")] public string Date { get; set; }
        [J("id")] public string Id { get; set; }
        [J("img")] public string Img { get; set; }
        [J("tags")] public string[] Tags { get; set; }
        [J("title")] public string Title { get; set; }
    }

    public partial class Pagination
    {
        [J("currentPage")] public long CurrentPage { get; set; }
        [J("hasNextPage")] public bool HasNextPage { get; set; }
        [J("nextPage")] public long NextPage { get; set; }
        [J("pages")] public long[] Pages { get; set; }
    }

    public partial class ApiMovies
    {
        public static ApiMovies FromJson(string json) => JsonConvert.DeserializeObject<ApiMovies>(json, Data.Converter.Settings);
    }
    #endregion

    partial class MovieData
    {
        [J("date")] public string Date { get; set; }
        [J("director")] public Director Director { get; set; }
        [J("genres")] public Genre[] Genres { get; set; }
        [J("gid")] public string Gid { get; set; }
        [J("id")] public string Id { get; set; }
        [J("imageSize")] public ImageSize ImageSize { get; set; }
        [J("img")] public string Img { get; set; }
        [J("producer")] public Producer Producer { get; set; }
        [J("publisher")] public Publisher Publisher { get; set; }
        [J("samples")] public Sample[] Samples { get; set; }
        [J("series")] public object Series { get; set; }
        [J("stars")] public Star[] Stars { get; set; }
        [J("title")] public string Title { get; set; }
        [J("uc")] public string Uc { get; set; }
        [J("videoLength")] public long VideoLength { get; set; }
    }

    partial class Director
    {
        [J("id")] public string Id { get; set; }
        [J("name")] public string Name { get; set; }
    }

    partial class Genre
    {
        [J("id")] public string Id { get; set; }
        [J("name")] public string Name { get; set; }
    }

    partial class ImageSize
    {
        [J("height")] public long Height { get; set; }
        [J("width")] public long Width { get; set; }
    }

    partial class Producer
    {
        [J("id")] public string Id { get; set; }
        [J("name")] public string Name { get; set; }
    }

    partial class Publisher
    {
        [J("id")] public string Id { get; set; }
        [J("name")] public string Name { get; set; }
    }

    partial class Sample
    {
        [J("alt")] public string Alt { get; set; }
        [J("id")] public string Id { get; set; }
        [J("src")] public string Src { get; set; }
        [J("thumbnail")] public string Thumbnail { get; set; }
    }

    partial class Star
    {
        [J("id", NullValueHandling = N.Ignore)] public string Id { get; set; }
        [J("name", NullValueHandling = N.Ignore)] public string Name { get; set; }
    }

    partial class MovieData
    {
        public static MovieData FromJson(string json) => JsonConvert.DeserializeObject<MovieData>(json, Data.Converter.Settings);
    }

    partial class Magnet
    {
        [J("hasSubtitle")] public bool HasSubtitle { get; set; }
        [J("id")] public string Id { get; set; }
        [J("isHD")] public bool IsHd { get; set; }
        [J("link")] public string Link { get; set; }
        [J("numberSize")] public long NumberSize { get; set; }
        [J("shareDate")] public string ShareDate { get; set; }
        [J("size")] public string Size { get; set; }
        [J("title")] public string Title { get; set; }
    }

    partial class Magnet
    {
        public static Magnet[] FromJson(string json) => JsonConvert.DeserializeObject<Magnet[]>(json, Data.Converter.Settings);
    }

    static class Serialize
    {
        public static string ToJson(this MovieData self) => JsonConvert.SerializeObject(self, Data.Converter.Settings);
        public static string ToJson(this Magnet[] self) => JsonConvert.SerializeObject(self, Data.Converter.Settings);
        public static string ToJson(this ApiMovies self) => JsonConvert.SerializeObject(self, Data.Converter.Settings);


    }

    static class Converter
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
