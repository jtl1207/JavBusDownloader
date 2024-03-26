using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data
{
    public partial class MovieData
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("director")]
        public Director Director { get; set; }

        [JsonProperty("genres")]
        public Genre[] Genres { get; set; }

        [JsonProperty("gid")]
        public string Gid { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("imageSize")]
        public ImageSize ImageSize { get; set; }

        [JsonProperty("img")]
        public string Img { get; set; }

        [JsonProperty("producer")]
        public Producer Producer { get; set; }

        [JsonProperty("publisher")]
        public Publisher Publisher { get; set; }

        [JsonProperty("samples")]
        public Sample[] Samples { get; set; }

        [JsonProperty("series")]
        public object Series { get; set; }

        [JsonProperty("stars")]
        public Star[] Stars { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("uc")]
        public string Uc { get; set; }

        [JsonProperty("videoLength")]
        public long VideoLength { get; set; }
    }

    public partial class Director
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Genre
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class ImageSize
    {
        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }
    }

    public partial class Producer
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Publisher
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Sample
    {
        [JsonProperty("alt")]
        public string Alt { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("src")]
        public string Src { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }
    }

    public partial class Star
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
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