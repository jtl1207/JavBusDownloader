// var movieData = MovieData.FromJson(jsonString);
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

namespace Data
{
    public partial class MovieData
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

    public partial class Director
    {
        [J("id")] public string Id { get; set; }
        [J("name")] public string Name { get; set; }
    }

    public partial class Genre
    {
        [J("id")] public string Id { get; set; }
        [J("name")] public string Name { get; set; }
    }

    public partial class ImageSize
    {
        [J("height")] public long Height { get; set; }
        [J("width")] public long Width { get; set; }
    }

    public partial class Producer
    {
        [J("id")] public string Id { get; set; }
        [J("name")] public string Name { get; set; }
    }

    public partial class Publisher
    {
        [J("id")] public string Id { get; set; }
        [J("name")] public string Name { get; set; }
    }

    public partial class Sample
    {
        [J("alt")] public string Alt { get; set; }
        [J("id")] public string Id { get; set; }
        [J("src")] public string Src { get; set; }
        [J("thumbnail")] public string Thumbnail { get; set; }
    }

    public partial class Star
    {
        [J("id", NullValueHandling = N.Ignore)] public string Id { get; set; }
        [J("name", NullValueHandling = N.Ignore)] public string Name { get; set; }
    }

    public partial class MovieData
    {
        public static MovieData FromJson(string json) => JsonConvert.DeserializeObject<MovieData>(json, Data.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this MovieData self) => JsonConvert.SerializeObject(self, Data.Converter.Settings);
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